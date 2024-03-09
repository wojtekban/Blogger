﻿using Application.Dto;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Filters;
using WebAPI.helpers;
using WebAPI.Wrappers;

namespace WebAPI.Controllers.V1
{
   // [ApiExplorerSettings(IgnoreApi = true)]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> Get([FromQuery] PaginationFilter paginationFilter)
        {
            var validPaginationFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var posts = await _postService.GetAllPostsAsync(validPaginationFilter.PageNumber, validPaginationFilter.PageSize);
            var totalRecords = await _postService.GetAllPostsCountAsync();

            //return Ok(new PagedResponse<IEnumerable<PostDto>>(posts, validPaginationFilter.PageNumber, validPaginationFilter.PageSize));
            return Ok(PaginationHelper.CreatePagedResponse(posts, validPaginationFilter, totalRecords));
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

        [SwaggerOperation(Summary = "Create a new post")]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostDto newPost)
        {
            var post = await _postService.AddNewPostAsync(newPost);
            return Created($"api/posts/{post.Id}", new Response<PostDto>(post));
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
        public async Task<IActionResult> SearachingPostAsync(string title, int pageNumber, int pageSize)
        {
            var searchingPost = await _postService.SearachingPostAsync(title, pageNumber, pageSize);
            return Ok(searchingPost);
        }
    }
}
