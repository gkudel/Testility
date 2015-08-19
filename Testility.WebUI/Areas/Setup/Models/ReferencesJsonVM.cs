using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Testility.Domain.Entities;

namespace Testility.WebUI.Areas.Setup.Models
{
    public class ReferencesJsonVM
    {
        public IQueryable<Reference> AvailableReferences { get; set; }
        public int[] SelectedReferencesIds { get; set; }
        public ReferencesJsonVM(IQueryable<Reference> availableRef, int[] selectedRef = null)
        {
            AvailableReferences = availableRef;
            SelectedReferencesIds = selectedRef;
        }

    }
}