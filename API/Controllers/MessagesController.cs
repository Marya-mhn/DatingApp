using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using API.Entities;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using API.Helpers;
using API.Extensions;

namespace API.Controllers
{
    public class MessagesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        public MessagesController(IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper)
        {
            _mapper = mapper;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto){
             var username = User.GetUsername();

             if(username == createMessageDto.RecipientUsername.ToLower()){
                return BadRequest("You cannot send messages to yourself");
             }

             var sender = await _userRepository.GetUserByUsernameAsync(username);
             var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername.ToLower());

             if(recipient == null){
                return NotFound();
             } 

             var message = new Message{
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
             };

            _messageRepository.AddMessage(message);

            if(await _messageRepository.SaveAllAsync()){
                return Ok(_mapper.Map<MessageDto>(message));
            }

            return BadRequest("Failed to send message");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams){
            messageParams.Username = User.GetUsername();

            var messages = await _messageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages));

            return messages;
        }
    }
}