using FluentValidation;
using FluentValidationApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FluentValidationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private static List<Customer> _customers = new List<Customer>();
        private static int _nextId = 1;
        private readonly IValidator<CustomerDto> _validator;

        public CustomersController(IValidator<CustomerDto> validator)
        {
            _validator = validator;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Customer>> GetAll()
        {
            return Ok(_customers);
        }

        [HttpGet("{id}")]
        public ActionResult<Customer> GetById(int id)
        {
            var customer = _customers.Find(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        public ActionResult<Customer> Create(CustomerDto customerDto)
        {
            // A validação automática ocorre por causa do ApiController
            // Mas podemos validar manualmente também
            var validationResult = _validator.Validate(customerDto);
            
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var customer = new Customer
            {
                Id = _nextId++,
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Email = customerDto.Email,
                PhoneNumber = customerDto.PhoneNumber,
                DateOfBirth = customerDto.DateOfBirth,
                Address = customerDto.Address
            };

            _customers.Add(customer);
            
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CustomerDto customerDto)
        {
            var existingCustomer = _customers.Find(c => c.Id == id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            // Validação manual
            var validationResult = _validator.Validate(customerDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            // Atualiza as propriedades
            existingCustomer.FirstName = customerDto.FirstName;
            existingCustomer.LastName = customerDto.LastName;
            existingCustomer.Email = customerDto.Email;
            existingCustomer.PhoneNumber = customerDto.PhoneNumber;
            existingCustomer.DateOfBirth = customerDto.DateOfBirth;
            existingCustomer.Address = customerDto.Address;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var customer = _customers.Find(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            _customers.Remove(customer);
            return NoContent();
        }
    }
} 