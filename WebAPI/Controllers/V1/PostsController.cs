using Application.Interfaces;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WebAPI.Filters;
using WebAPI.Helpers;
using WebAPI.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Identity;
using Application.Dto;
using WebAPI.Attributes;
using Microsoft.Extensions.Caching.Memory;
using WebAPI.Cache;


namespace WebAPI.Controllers.V1
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger _logger;

        public PostsController(IPostService postService, IMemoryCache memoryCache, ILogger<PostsController> logger)
        {
            _postService = postService;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        [SwaggerOperation(Summary = "Retrieves sort fields")]
        [HttpGet("[action]")]
        public IActionResult GetSortFields()
        {
            return Ok(SortingHelper.GetSortFields().Select(x => x.Key));
        }

        [SwaggerOperation(Summary = "Retrieves paged posts")]
        [Cached(600)]
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationFilter paginationFilter, [FromQuery] SortingFilter sortingFilter, [FromQuery] string filterBy = "")
        {
            var validPaginationFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var validSortingFilter = new SortingFilter(sortingFilter.SortField, sortingFilter.Ascending);

            var posts = await _postService.GetAllPostsAsync(validPaginationFilter.PageNumber, validPaginationFilter.PageSize,
                                                       validSortingFilter.SortField, validSortingFilter.Ascending,
                                                       filterBy);
            var totalRecords = await _postService.GetAllPostCountAsync(filterBy);

            return Ok(PaginationHelper.CreatePagedResponse(posts, validPaginationFilter, totalRecords));
        }

        [SwaggerOperation(Summary = "Retrieves all posts")]
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("[action]")]
        [EnableQuery]
        public IQueryable<PostDto> GetAll()
        {
            var posts = _memoryCache.Get<IQueryable<PostDto>>("posts");
            if (posts == null)
            {
                _logger.LogInformation("Fetching from service.");
                posts = _postService.GetAllPosts();
                _memoryCache.Set("posts", posts, TimeSpan.FromMinutes(1));
            }
            else
            {
                _logger.LogInformation("Fetching from cache");
            }



            return posts;
        }

        [SwaggerOperation(Summary = "Retrievers a specific post by unique id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(new Response<PostDto>(post));
        }
        [ValidateFilter]
        [SwaggerOperation(Summary = "Create a new post")]
        [Authorize(Roles = UserRoles.User)]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostDto newPost)
        {
            //var validator = new CreatePostDtoValidator();
            //var result = validator.Validate(newPost);
            //if (!result.IsValid)
            //{
            //    return BadRequest(new Response<bool>
            //    {
            //        Succeeded = false,
            //        Message = "Something went wrong.",
            //        Errors = result.Errors.Select(x => x.ErrorMessage)
            //    });
            //}

            var post = await _postService.AddNewPostAsync(newPost, User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Created($"api/posts/{post.Id}", new Response<PostDto>(post));
        }

        [SwaggerOperation(Summary = "Update a exsisting post")]
        [Authorize(Roles = UserRoles.User)]
        [HttpPut]
        public async Task<IActionResult> Update(UpdatePostDto updatePost)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(updatePost.Id, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!userOwnsPost)
            {
                return BadRequest(new Response(false,"You do not own this post" ));
            }

            await _postService.UpdatePostAsync(updatePost);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete a specific post")]
        [Authorize(Roles = UserRoles.AdminOrUser)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
            var isAdmin = User.FindFirstValue(ClaimTypes.Role).Contains(UserRoles.Admin);

            if (!isAdmin && !userOwnsPost)
            {
                return BadRequest(new Response(false, "You do not own this post" ));
            }

            await _postService.DeletePostAsync(id);
            return NoContent();
        }
    }
}
