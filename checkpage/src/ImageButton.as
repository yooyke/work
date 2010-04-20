package  
{
	import flash.display.SimpleButton;
	import flash.display.Loader;
	import flash.events.Event;
	import flash.events.ProgressEvent;
	import flash.net.URLRequest;
	import flash.system.LoaderContext;

	/**
	 * ...
	 * @author Yusuke Kikkawa
	 */
	public class ImageButton extends SimpleButton
	{
		private var _id:String;
		private var _loaderU:Loader = new Loader();
		private var _loaderD:Loader = new Loader();
		private var _loading:Boolean = false;

		public function get id():String { return _id; }
		public function get loading():Boolean { return _loading; }
		
		public function ImageButton(id:String) 
		{
			this._id = id;
			this.upState = _loaderU;
			this.overState = _loaderD;
			this.downState = _loaderD;
			this.hitTestState = _loaderD;
		}

		public function loadImage(upPath:String, downPath:String):void
		{
			if (_loading)
				return;
			
			_loading = true;
			_loaderU.load(new URLRequest(upPath));
			_loaderD.load(new URLRequest(downPath));
		}
	}
}