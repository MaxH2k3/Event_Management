using Event_Management.Application.Message;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Validators
{
    public class EventRequestDtoValidator : AbstractValidator<EventRequestDto>
    {
        public EventRequestDtoValidator()
        {

            RuleFor(x => x.EventName)
                //.NotEmpty().WithMessage("EventName is required!")
                .Length(3, 250).WithMessage("Event name must be between 3 and 250 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Event Description is required!")
                .Length(3, 5000).WithMessage("Event Description must be between 3 and 5000 characters.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("StartDate is required.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("EndDate is required.");
                
            RuleFor(x => x.Location.Name)
                .NotEmpty().WithMessage("Location is required!")
                .Length(5, 500).WithMessage("Location name must be between 5 and 500 characters");

            /*RuleFor(x => x.Location.LocationId)
                .NotEmpty().WithMessage("LocationId is required!");*/

            RuleFor(x => x.Location.Address)
                .NotEmpty().WithMessage("LocationAddress is required!");

            RuleFor(x => x.Location.Coord)
                .NotEmpty().WithMessage("LocationCoord is required!");

            RuleFor(x => x.Capacity)
                .NotEmpty().WithMessage("Capacity is required.")
                .GreaterThan(-1).WithMessage("Capacity must be greater than -1.");

            RuleFor(x => x.Ticket)
                .InclusiveBetween(0, 5000000).WithMessage("Maximum ticket price is 5 000 000.");
        }
    }
}
