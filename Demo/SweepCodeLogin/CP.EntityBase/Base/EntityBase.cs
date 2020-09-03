using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CP.EntityBase
{
    public abstract class EntityBase: ICloneable
    {
        public Map EnMap = null;
        


        public EntityBase()
        {
            EnMap = CreateEnMap();
        }

        /// <summary>
        /// 子类需要继承
        /// </summary>
        public abstract Map CreateEnMap();


        public object Clone()
        {
            return this.MemberwiseClone(); //浅复制
        }
    }
}
