
using MySql.Data.MySqlClient;
using System.Data;
using DatingWebApp.DTOs;
using DatingWebApp.Data;
using DatingWebApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using DatingWebApp.Entities;



public class AdminRepository : IAdminRepository

{

    private readonly Db_Context _context;



    public AdminRepository(Db_Context context)

    {

        _context = context;

    }



    public async Task<List<PhotoApprovalStatsDto>> GetPhotoApprovalCountAsync()

    {

        var result = new List<PhotoApprovalStatsDto>();



       await using var command = _context.Database.GetDbConnection().CreateCommand();

        command.CommandText = "CALL GetPhotoApprovalCounts()";

        command.CommandType = CommandType.Text;

        if (command.Connection.State != ConnectionState.Open)
           { await command.Connection.OpenAsync(); }

        await using var reader = await command.ExecuteReaderAsync();


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



        await using var command = _context.Database.GetDbConnection().CreateCommand();

        command.CommandText = "CALL GetUsersWithoutMainPhoto()";

        command.CommandType = CommandType.Text;

        if (command.Connection.State != ConnectionState.Open)
        { await command.Connection.OpenAsync(); }

        await using var reader = await command.ExecuteReaderAsync();



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