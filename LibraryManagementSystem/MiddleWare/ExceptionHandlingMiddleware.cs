using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace LibraryManagement.MiddleWare;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next=next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(DbUpdateException ex)
        {
            var sqlEx = ex.InnerException as SqlException;

            if(sqlEx?.Number == 547)
            {
                if (ex.Entries.Count > 0 && ex.Entries[0].State == EntityState.Added)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { message = "The referenced record does not exist." });
                }
                else if(ex.Entries.Count > 0 && ex.Entries[0].State == EntityState.Deleted){
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    await context.Response.WriteAsJsonAsync(new { message = "This record is still referenced by other data and cannot be deleted." });
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    await context.Response.WriteAsJsonAsync(new { message = "This operation violates a database constraint." });
                }
            }
            else if (sqlEx?.Number == 2601 || sqlEx?.Number == 2627)
            {
                context.Response.StatusCode= StatusCodes.Status409Conflict;
                await context.Response.WriteAsJsonAsync(new { message = "A record with the same unique value already exists."});
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { message = "A database error occurred." });
            }
    }
            catch (Exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { message = "An unexpected error occurred." });
        }
    
    }
}