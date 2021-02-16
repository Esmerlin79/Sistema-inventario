using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sistema.Datos;
using Sistema.Entidades.Usuarios;
using Sistema.Web.Models.Usuarios.Usuario;

namespace Sistema.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly DbContextSistema _context;
        private readonly IConfiguration _config;

        public UsuariosController(DbContextSistema context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        [Authorize(Roles = "Administrador")]
        // GET: api/Usuarios/Listar 
        [HttpGet("[action]")]
        public async Task<IEnumerable<UsuarioViewModel>> Listar()
        {
            var user = await _context.Usuario.Include(x => x.rol).ToListAsync();

            return user.Select(x => new UsuarioViewModel
            {
                idusuario = x.idusuario,
                idrol = x.idrol,
                rol = x.rol.nombre,
                nombre = x.nombre,
                tipo_documento = x.tipo_documento,
                num_documento = x.num_documento,
                direccion = x.direccion,
                telefono = x.telefono,
                email = x.email,
                password_hash = x.password_hash,
                condicion = x.condicion
            });

        }

        [Authorize(Roles = "Administrador")]
        // POST: api/Usuarios/Crear
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var email = model.email.ToLower();
            if(await _context.Usuario.AnyAsync(x => x.email == email))
            {
                return BadRequest("El email ya existe");
            }
            var rolExist = await _context.rol.FirstOrDefaultAsync(x => x.idrol == model.idrol);
            if (rolExist == null)
            {
                return BadRequest("El Rol Especificado no existe");
            }
            this.CreatePasswordHash(model.password, out byte[] passwordHash, out byte[] passwordSalt);
            Usuario obj = new Usuario
            {
                idrol = model.idrol,
                nombre = model.nombre,
                tipo_documento = model.tipo_documento,
                num_documento = model.num_documento,
                direccion = model.direccion,
                telefono = model.telefono,
                email = model.email.ToLower(),
                password_hash = passwordHash,
                password_salt = passwordSalt,
                condicion = true
            };
            _context.Usuario.Add(obj);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }
        //out son variables que se declaran y se usan en otro metodo loca
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        [Authorize(Roles = "Administrador")]
        // PUT: api/Usuarios/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idusuario <= 0)
            {
                return BadRequest();
            }
            var user = await _context.Usuario.FirstOrDefaultAsync(x => x.idusuario == model.idusuario);
            if (user == null)
            {
                return NotFound();
            }
                user.idrol = model.idrol;
                user.nombre = model.nombre;
                user.tipo_documento = model.tipo_documento;
                user.num_documento = model.num_documento;
                user.direccion = model.direccion;
                user.telefono = model.telefono;
                user.email = model.email;

            if (model.act_password)
            {
                this.CreatePasswordHash(model.password, out byte[] passHash, out byte[] passSalt);
                user.password_hash = passHash;
                user.password_salt = passSalt;
            }

            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        [Authorize(Roles = "Administrador")]
        // PUT: api/Usuarios/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var users = await _context.Usuario.FirstOrDefaultAsync(x => x.idusuario == id);
            if (users == null)
            {
                return NotFound();
            }
            users.condicion = false;

            _context.Entry(users).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        [Authorize(Roles = "Administrador")]
        // PUT: api/Usuarios/Activar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var users = await _context.Usuario.FirstOrDefaultAsync(x => x.idusuario == id);
            if (users == null)
            {
                return NotFound();
            }
            users.condicion = true;

            _context.Entry(users).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }
        
        // POST: api/Usuarios/Login
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var email = model.email.ToLower();
            var usuario = await _context.Usuario.Where(x => x.condicion == true).Include(x => x.rol).FirstOrDefaultAsync(x => x.email == email);

            if(usuario == null)
            {
                return NotFound();
            }
            if (!this.verificarPasswordHash(model.password, usuario.password_hash, usuario.password_salt))
            {
                return NotFound();
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.idusuario.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, usuario.rol.nombre),
                new Claim("idusuario",usuario.idusuario.ToString()),
                new Claim("rol",usuario.rol.nombre),
                new Claim("nombre",usuario.nombre)
            };
            return Ok(new { token = this.GenerarToken(claims) });

        }
        public string GenerarToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds,
                claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private bool verificarPasswordHash(string password, byte[] passHashAlmacenado, byte[] passSalt)
        {
            using(var hmc = new HMACSHA512(passSalt))
            {
                var passHashNuevo = hmc.ComputeHash(Encoding.UTF8.GetBytes(password));
                return new ReadOnlySpan<byte>(passHashAlmacenado).SequenceEqual(new ReadOnlySpan<byte>(passHashNuevo));
            }
        }
        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.idusuario == id);
        }
    }
} 