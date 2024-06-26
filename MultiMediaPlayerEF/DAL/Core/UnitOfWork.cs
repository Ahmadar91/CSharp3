﻿using System;

namespace DAL.Core
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly Context _context;
        public IAlbumRepository Album { get; }
        public IPlayListItemRepository PlayList { get; }

        public UnitOfWork()
        {
            _context = new Context();
            _context.Database.EnsureCreated();
            Album = new AlbumRepository(_context);
            PlayList = new PlayListItemRepository(_context);

        }
        public UnitOfWork(Context context)
        {
            _context = context;
            Album = new AlbumRepository(_context);
            PlayList = new PlayListItemRepository(_context);
        }
        /// <summary>
        /// Save the changes for a Entity
        /// </summary>
        public void Complete()
        {
            _context.SaveChanges();
        }
        /// <summary>
        /// Dispose the context
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}