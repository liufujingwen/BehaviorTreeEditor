using System;

namespace Serializable.Proxy
{
	public interface IProxy
	{
		object getValue (Context ctx, byte flag) ;

		void setValue (Context ctx, object value) ;
	}
}

