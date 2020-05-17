using System;
using System.Collections.Generic;
using System.Text;

namespace FilmCatalog.Data.Models.Contracts
{
    public interface IRating
    {
        int Id { get; set; }

        double Score { get; set; }

        int MovieId { get; set; }

        Movie Movie { get; set; }

        User User { get; set; }
    }
}
