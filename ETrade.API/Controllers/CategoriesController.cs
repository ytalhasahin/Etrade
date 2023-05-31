using Etrade.DAL.Abstract;
using Etrade.Entity.Models.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ETrade.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        ICategoryDAL _categoryDal;

        public CategoriesController(ICategoryDAL categoryDal)
        {
            _categoryDal = categoryDal;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_categoryDal.GetAll());
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if(id == 0 || _categoryDal.GetAll() == null)
            {
                return BadRequest("Kategori bilgisi alınamadı.");
            }
            var category = _categoryDal.Get(id);
            if(category == null)
            {
                return NotFound("Kategori bulunamadı.");
            }
            return Ok("Başarılı");
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public IActionResult Post([FromBody] Category category)
        {
            if(ModelState.IsValid)
            {
                _categoryDal.Add(category);
                return Ok();
                //return CreatedAtAction("Get", new {id=category.Id},category);
            }
            return BadRequest();
        }

        // PUT api/<CategoriesController>/5
        [HttpPut]
        public IActionResult Put([FromBody] Category category)
        {
            var model = _categoryDal.Get(category.Id);
            model.Name = category.Name;
            model.Description = category.Description;
            if(ModelState.IsValid)
            {
                _categoryDal.Update(model);
                return Ok(model);
            }
            return BadRequest();
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
