using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Via.Application.Features.Denemes.Comments.Create;

public class DenemeCreateCommendValid : AbstractValidator<DenemeCreateCommendRequest>
{
    public DenemeCreateCommendValid()
    {
        RuleFor(i => i.Name).NotEmpty().NotNull().MinimumLength(4);
    }
}
