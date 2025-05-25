
using MySql.Data.MySqlClient;
using System.Data;
using DatingWebApp.DTOs;
using DatingWebApp.Data;
using DatingWebApp.Interfaces;
using Microsoft.EntityFrameworkCore;



public class AdminRepository : IAdminRepository

{

    private readonly Db_Context _context;



    public AdminRepository(Db_Context context)

    {

        _context = context;

    }



    public async Task<List<PhotoApprovalStatsDto>> GetPhotoApprovalStatsAsync()

    {

        var result = new List<PhotoApprovalStatsDto>();



        using var connection = _context.Database.GetDbConnection();

        await connection.OpenAsync();



        using var command = connection.CreateCommand();

        command.CommandText = "CALL GetPhotoApprovalCounts()";

        command.CommandType = CommandType.Text;



        using var reader = await command.ExecuteReaderAsync();



        while (await reader.ReadAsync())

        {

            result.Add(new PhotoApprovalStatsDto

            {

                Username = reader.GetString("Username"),

                ApprovedPhotos = reader.GetInt32("ApprovedPhotos"),

                UnapprovedPhotos = reader.GetInt32("UnapprovedPhotos")

            });

        }



        return result;

    }



    public async Task<List<UserWithoutMainPhotoDto>> GetUsersWithoutMainPhotoAsync()

    {

        var result = new List<UserWithoutMainPhotoDto>();



        using var connection = _context.Database.GetDbConnection();

        await connection.OpenAsync();



        using var command = connection.CreateCommand();

        command.CommandText = "CALL GetUsersWithoutMainPhoto()";

        command.CommandType = CommandType.Text;



        using var reader = await command.ExecuteReaderAsync();



        while (await reader.ReadAsync())
        {

            result.Add(new UserWithoutMainPhotoDto

            {

                Username = reader.GetString("Username")

            });

        }



        return result;
    }
}