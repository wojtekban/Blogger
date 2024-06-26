﻿using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Application.Dto;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class PostServices : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PostServices(IPostRepository postRepository, IMapper mapper, ILogger<PostServices> logger)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public IQueryable<PostDto> GetAllPosts()
        {
            var posts = _postRepository.GetAll();
            return _mapper.ProjectTo<PostDto>(posts);
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy)
        {
            _logger.LogDebug("Fetching posts.");
            _logger.LogInformation($"pageNumber: {pageNumber} | pageSize: {pageSize}");

            var posts = await _postRepository.GetAllAsync(pageNumber, pageSize, sortField, ascending, filterBy);
            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }
        public async Task<int> GetAllPostCountAsync(string filterBy)
        {
            return await _postRepository.GetAllCountAsync(filterBy);
        }

        public async Task<PostDto> GetPostByIdAsync(int id)

        {
            var post = await _postRepository.GetByIdAsync(id);
            return _mapper.Map<PostDto>(post);
        }
        public async Task<PostDto> AddNewPostAsync(CreatePostDto newPost, string userId)
        {
            //if (string.IsNullOrEmpty(newPost.Title) && string.IsNullOrWhiteSpace(newPost.Title)) // sprawdzanie czy tytuł nie jest nullem lub ciągiem znaków
            //{
            //    throw new Exception("Post can not have an empty title.");
            //}
            //if (5 < newPost.Title.Length && newPost.Title.Length < 100)
            //{
            //    throw new Exception("The title must be beetween 5 and 100 charakters long");
            //}

            var post = _mapper.Map<Post>(newPost);
            post.UserId = userId;
            var result = await _postRepository.AddAsync(post);
            return _mapper.Map<PostDto>(result);
        }

        public async Task UpdatePostAsync(UpdatePostDto updatePost)
        {
            var existingPost = await _postRepository.GetByIdAsync(updatePost.Id);
            var post = _mapper.Map(updatePost, existingPost);
            await _postRepository.UpdateAsync(post);
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            await _postRepository.DeleteAsync(post);
        }

        public async Task<bool> UserOwnsPostAsync(int postId, string userId)
        {
            var post = await _postRepository.GetByIdAsync(postId);

            if (post == null)
            {
                return false;
            }

            if (post.UserId != userId)
            {
                return false;
            }

            return true;
        }
    }
}
