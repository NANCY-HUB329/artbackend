using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arts.Data;
using Arts.Models;
using Arts.Models.Dtos;
using Arts.Services.IService;
using AutoMapper;
namespace Arts.Services
{
    public class ArtService : IArtService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ArtService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Art>> GetAllArts()
        {
            var arts = await _dbContext.Arts.ToListAsync();

            // Update status for each art based on the current time and EndTime
            foreach (var art in arts)
            {
                if (DateTime.Now > art.EndTime)
                {
                    art.status = "closed";
                }
            }

            return arts;
        }

        public async Task<Art> GetArtById(Guid Id)
        {
            return await _dbContext.Arts.Where(b=>b.Id==Id).FirstOrDefaultAsync();   
        }

        public async Task<string> AddArt(Art art)
        {
            _dbContext.Arts.Add(art);
            await _dbContext.SaveChangesAsync();
            return "Art added successfully";
        }


        public async Task<string> UpdateArt()
        {

            await _dbContext.SaveChangesAsync();
            return "Art Updated Successfully";
        }


        public async Task<string> DeleteArt(Art art)
        {

            _dbContext.Arts.Remove(art);
            await _dbContext.SaveChangesAsync();
            return "Art deleted succesfully";
        }

        public async Task<List<Art>> GetOpenArts()
        {
           return await _dbContext.Arts.Where(b => b.status == "open").ToListAsync();
        }

        public async Task<List<Art>> GetClosedArts()
        {
            return await _dbContext.Arts.Where(b => b.status == "closed").ToListAsync();
        }
    }
}