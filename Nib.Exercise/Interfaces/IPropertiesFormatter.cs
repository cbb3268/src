using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nib.Exercise.Interfaces
{
    public interface IPropertiesFormatter<in TResource>
        where TResource : class
    {
        void Format(TResource resource);
    }
}
