using Application.Dto;
using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers.V2
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }
        [SwaggerOperation(Summary = "Retrieves all posts")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var posts = await _postService.GetAllPostsAsync();
            return Ok(
                new
                {
                    Posts = posts,
                    Count = posts.Count()
                });
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

            return Ok(post);
        }

        [SwaggerOperation(Summary = "Create a new post")]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostDto newPost)
        {
            var post = await _postService.AddNewPostAsync(newPost);
            return Created($"api/posts/{post.Id}", post);
        }

        [SwaggerOperation(Summary = "Update a existing post")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdatePostDto updatePost)
        {
            await _postService.UpdatePostAsync(updatePost);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete a specific post")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _postService.DeletePostAsync(id);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Searching specific title")]
        [HttpGet("Search/{title}")]
        public async Task<IActionResult> SearachingPostAsync(string title)
        {
            var searchingPost = await _postService.SearachingPostAsync(title);
            return Ok(searchingPost);
        }
    }
}
