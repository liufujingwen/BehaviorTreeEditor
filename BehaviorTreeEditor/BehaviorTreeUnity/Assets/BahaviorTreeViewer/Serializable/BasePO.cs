using System;
using System.Collections.Generic;
using System.Text;

namespace Serializable
{
    public abstract class BasePO
    {
        public byte[] SerializeObject<T>(Transfer transfer)
        {
            return transfer.Encode(GetPO());
        }

        public abstract PO GetPO();
        public virtual PO GetPO(PO po) { return po; }
        public abstract void SetData(PO po);
    }
}
