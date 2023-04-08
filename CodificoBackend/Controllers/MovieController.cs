using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using Microsoft.AspNetCore.Cors;
using System.Linq;
using Microsoft.Extensions.Logging;
using CodificoBackend.Models;

namespace CodificoBackend.Controllers
{
    [Route("Movie")]
    public class MovieController : Controller
    {
        private DataContext dataContext;
        private readonly ILogger<MovieController> _logger;

        public MovieController(DataContext _dataContext, ILogger<MovieController> logger)
        {
            dataContext = _dataContext;
            _logger = logger;
        }

        // GET: Movie/GetAllMovies
        [HttpGet("GetAllMovies")]
        public async Task<IActionResult> GetAllMovies()
        {
            _logger.LogInformation("********************        Ingresando a GetAllMovies          *****************");
            try
            {
                _logger.LogDebug("aa");
                _logger.LogInformation("+++++ Obteniendo todos las Movies de la base de datos  ++++++++");
                var result = await dataContext.Movie.ToListAsync();
                _logger.LogInformation("Resultados: ");
                _logger.LogInformation(result.Count().ToString());

                if (result != null)
                {
                    _logger.LogInformation("Se retorna Ok");
                    return Ok(result);
                }
                else
                {
                    _logger.LogInformation("-------- Error, Movies no encontradas   --------");
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("--------- Hubo una exepción: ");
                _logger.LogInformation(e.Message.ToString());
                Console.WriteLine(e.Message);
                return StatusCode(404, "Hubo un problema buscando a todos las Movies");
            }
        }

        // POST: Movie/SaveMovie       
        [HttpPost("SaveMovie")]
        public async Task<ActionResult<Movie>> SaveMovie([FromBody] Movie movie)
        {
            _logger.LogInformation("********************        Ingresando a SaveMovie          *****************");
            _logger.LogInformation("Se recibe la movie: ");
            LogMovieInformationWithId(movie);

            if (movie == null)
            {
                _logger.LogInformation("-------- Error, Movie no encontrada   --------");
                return NotFound();
            }

            try
            {
                _logger.LogInformation("+++++ Guardando la movie en la base de datos  ++++++++");
                dataContext.Movie.Add(movie);
                await dataContext.SaveChangesAsync();
                _logger.LogInformation("Se retorna Ok");
                return Ok(movie);
            }
            catch (Exception e)
            {
                _logger.LogInformation("--------- Hubo una exepción: ");
                _logger.LogInformation(e.Message.ToString());
                Console.WriteLine(e.Message);
                return StatusCode(404, "Hubo un problema guardando la movie");
            }
        }

        private void LogMovieInformationWithId(Movie result)
        {
            _logger.LogInformation("MovieId: {result.MovieId}", result.MovieId);
            LogMovieInformation(result);
        }

        private void LogMovieInformation(Movie result)
        {
            _logger.LogInformation("Resultados: ");
            _logger.LogInformation("MovieTitle {result.MovieTitle}", result.MovieTitle);
            _logger.LogInformation("MovieYear {result.MovieYear}", result.MovieYear);
            _logger.LogInformation("MovieGenre {result.MovieGenre}", result.MovieGenre);            
        }

        // DELETE: Movie/DeleteMovie/5
        [HttpDelete("DeleteMovie/{id}")]
        public async Task<ActionResult<Movie>> DeleteMovie(int id)
        {
            _logger.LogInformation("********************        Ingresando a DeleteMovie          *****************");
            _logger.LogInformation("Se recibe el {id}", id);

            try
            {
                _logger.LogInformation("+++++ Buscando la Movie en la base de datos  ++++++++");
                var movie = await dataContext.Movie.FindAsync(id);

                if (movie == null)
                {
                    _logger.LogInformation("-------- Error, Movie no encontrada   --------");
                    return NotFound();
                }

                dataContext.Movie.Remove(movie);
                await dataContext.SaveChangesAsync();
                _logger.LogInformation("Movie elimnada. Se retorna Ok");
                return Ok(movie);
            }
            catch (Exception e)
            {
                _logger.LogInformation("--------- Hubo una exepción: ");
                _logger.LogInformation(e.Message.ToString());
                Console.WriteLine(e.Message);
                return StatusCode(404, "Hubo un problema al eliminar la Movie");
            }
        }


    }
}
