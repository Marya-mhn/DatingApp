using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using API.DTOs;
using API.Entities;
using API.Data;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        public MessageRepository(DataContext context)
        {
            _context = context;
        }
        
        public void AddMessage(Message message){
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message){
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id){
            return await _context.Messages.FindAsync(id);
        }

        public async Task<bool> SaveAllAsync(){
            return await _context.SaveChangesAsync() > 0
        }
    }
}