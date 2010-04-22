package  
{
	import flash.display.Loader;
	import flash.net.URLRequest;
	/**
	 * ...
	 * @author Yusuke Kikkawa
	 */
	public class Image extends Loader
	{
		private var _loading:Boolean = false;

		public function get loading():Boolean { return _loading; }
		
		public function Image() 
		{
		}

		public function loadImage(path:String):void
		{
			if (_loading)
				return;
			
			_loading = true;
			super.load(new URLRequest(path));
		}
	}
}