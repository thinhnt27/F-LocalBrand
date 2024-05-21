using AutoMapper;
using F_LocalBrand.Dtos;
using F_LocalBrand.Models;
using F_LocalBrand.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace F_LocalBrand.Service
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _unitOfWork.User.GetByIdAsync(id);
        }

        public async Task<UserModel?> GetUserByEmail(string email)
        {
            var user = await _unitOfWork.User.FindUserByEmail(email);
            return _mapper.Map<UserModel>(user);
        }


    }
}
