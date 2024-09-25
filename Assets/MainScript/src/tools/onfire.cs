using System;

public class onFire:SingleTonMonoBehaviour<onFire>
{
      public class Listener
      {
            public Action<object> cb;
            public bool once = false;
      }

      private readonly TSMap<string, TSArray<Listener>> es = new TSMap<string, TSArray<Listener>>();

      public  void on(string eventName, Action<object> cb, bool once = false)
      {
            if (this.es.get(eventName) == null)
            {
                this.es[eventName] = new TSArray<Listener>();
            }
            
            this.es[eventName].push(new Listener()
            {
              cb = cb,
              once = once,
            });
      }
      
      public void once(string eventName, Action<object> cb)
      {
            this.on(eventName, cb, true);
      }
      
      public void fire(string eventName, object args = null)
      {
            TSArray<Listener> listeners = this.es.get(eventName);
            if (listeners == null) return;
			
            int l = listeners.length;
            for(int i = l - 1; i >= 0; i--)
            {
				Listener ml = listeners[i];
				ml.cb(args);
				if (ml.once)
				{
					off(eventName, ml.cb);
				}
            }
      }
		
      public void off(string eventName, Action<object> cb)
      {
			var listeners = this.es.get(eventName);
			if(listeners != null)
			{
				var l = listeners.length;
				for (int i = l - 1; i >= 0; i--)
				{
					if (listeners[i].cb == cb)
					{
						listeners.RemoveAt(i);
					}
				}
			}
	  }
	
      public void emit(string eventName, object args = null)
      {
            this.fire(eventName, args);
      }
}
