using System;
using System.Collections.Generic;

namespace NatechAssignmentDataLayer.Models;

public class LocateHeader
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public ICollection<LocateDetails> Details { get; set; }

    public LocateHeader()
    {

    }

    public LocateHeader(Guid keyGuid, DateTime created)
    {
        Id = keyGuid;
        Created = created;
    }
}