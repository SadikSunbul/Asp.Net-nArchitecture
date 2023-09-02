using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Via.Application.Features.Denemes.Constants;
using Via.Application.Services.Repositories;
using Via.Domain.Entities;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Exceptions.Types;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Rules;

namespace Via.Application.Features.Denemes.Rules;

public class DenemeBusniessRules : BaseRules
{
    private readonly IDenemeRepository denemeRepository;

    public DenemeBusniessRules(IDenemeRepository denemeRepository)
    {
        this.denemeRepository = denemeRepository;
    }

    public async Task TrialNameUniquenessCheckAsync(string name)
    {
        var data = await denemeRepository.AnyAsync(i => i.Name == name);
        if (data)
        {
            throw new BusniesException(DenemeMessage.DenemeNameExists);
        }
        return;
    }

    public async Task<Deneme> IsThereSuchAnElementTo(Guid Id)
    {
        var data = await denemeRepository.GetAsync(i => i.Id == Id);
        if (data == null)
        {
            throw new BusniesException(DenemeMessage.DenemeThereİsNoSuchItem);
        }
        return data;
    }
}
