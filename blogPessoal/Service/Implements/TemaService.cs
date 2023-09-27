﻿using blogPessoal.Data;
using blogPessoal.Model;
using Microsoft.EntityFrameworkCore;

namespace blogPessoal.Service.Implements
{
    public class TemaService : ITemaService
    {

        private readonly AppDbContext _context;

        public TemaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tema>> GetAll()
        {
            return await _context.Tema
                .Include(t => t.Postagem) // Incluindo lista das postagens           
                .ToListAsync();
        }

        public async Task<Tema?> GetById(long id)
        {
            try
            {
                var tema = await _context.Tema
                  .Include(t => t.Postagem)
                  .FirstAsync(i => i.Id == id);

                return tema;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Tema>> GetByDescricao(string descricao)
        {
            var tema = await _context.Tema
                .Include(t => t.Postagem)
                .Where(p => p.Descricao.Contains(descricao))
                .ToListAsync();

            return tema;
        }

        public async Task<Tema?> Create(Tema tema)
        {
            await _context.Tema.AddAsync(tema);
            await _context.SaveChangesAsync();

            return tema;

        }

        public async Task<Tema?> Update(Tema tema)
        {
            var temaUpdate = await _context.Postagens.FindAsync(tema.Id);

            if (temaUpdate is null)
            {
                return null;
            }
            _context.Entry(temaUpdate).State = EntityState.Detached;
            _context.Entry(tema).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return tema;
        }
        public async Task Delete(Tema tema)
        {
            _context.Tema.Remove(tema);
            await _context.SaveChangesAsync();
        }

    }
}
