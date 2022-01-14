﻿using AutoMapper;
using Data.DataModel;
using Data.Uow;
using Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartWasteCollectionSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly ILogger<VehicleController> logger;

        private readonly IMapper mapper;

        public VehicleController(ILogger<VehicleController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var listOfVehicle = unitOfWork.Vehicle.GetAll();
            unitOfWork.Complate();

            var entities = mapper.Map<IEnumerable<Vehicle>, IEnumerable<VehicleEntity>>(listOfVehicle);

            return Ok(entities);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var vehicle = unitOfWork.Vehicle.GetById(id);
            if (vehicle is null)
            {
                return NotFound();
            }
            unitOfWork.Complate();

            var entity = mapper.Map<Vehicle, VehicleEntity>(vehicle);
            return Ok(entity);
        }

        [HttpPost]
        public IActionResult Post([FromBody] VehicleEntity entity)
        {
            var vehicle = mapper.Map<VehicleEntity, Vehicle>(entity);
            var response = unitOfWork.Vehicle.Add(vehicle);

            unitOfWork.Complate();

            return CreatedAtAction("Post", response);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] VehicleEntity entity, long id)
        {
            var vehicle = mapper.Map<VehicleEntity, Vehicle>(entity);

            vehicle.Id = id;
            var response = unitOfWork.Vehicle.Update(vehicle);

            unitOfWork.Complate();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var response = unitOfWork.Vehicle.Delete(id);

            unitOfWork.Complate();

            return Ok();
        }
    }
}
