package  
{
	import flash.system.LoaderContext;
	import flash.display.Loader;
	import flash.events.Event;
	import flash.events.ProgressEvent;
	import flash.net.URLRequest;

	/**
	 * ...
	 * @author Yusuke Kikkawa
	 */
	public class BGLoader extends Loader
	{
		private var _queue:Queue = new Queue();
		private var _loading:Boolean = false;
		
		public function get loading():Boolean { return _loading; }
		
		public function BGLoader() 
		{
            this.contentLoaderInfo.addEventListener(Event.COMPLETE, onLoaded);
		}

		public function loadBG(url:String, context:LoaderContext = null):void 
		{
			if (_loading)
			{
				trace("enqueue");
				_queue.enqueue(url);
				return;
			}
			
			_loading = true;
			super.load(new URLRequest(url), context);
		}
		
		private function onLoaded(event:Event):void
		{
			_loading = false;
			
			if (_queue.length > 0)
				loadBG(_queue.dequeue());
        }
	}
}