﻿using System.Data.Common;
using AutoMapper;
using DatingWebApp.Interfaces;

namespace DatingWebApp.Data
{
    public class UnitOfWork(Db_Context context, IUserRepository userRepository, 
        ILikesRepository likesRepository,IMessageRepository messageRepository,
        IPhotoRepository photoRepository):IUnitOfWork
    {
       
        public IUserRepository UserRepository =>userRepository;

        public IMessageRepository MessageRepository => messageRepository;

        public ILikesRepository LikesRepository => likesRepository;

        public IPhotoRepository PhotoRepository => photoRepository;

        public async Task<bool> Complete()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return context.ChangeTracker.HasChanges();
        }
    }
}
