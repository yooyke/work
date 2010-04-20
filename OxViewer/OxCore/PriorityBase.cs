namespace OxCore
{
    public enum PriorityBase
    {
        /// <summary>
        /// 990
        /// </summary>
        IO = 990,
        /// <summary>
        /// 1000
        /// </summary>
        Controller = 1000,
        /// <summary>
        /// 1010
        /// </summary>
        ControllerProtocol = 1010,
        /// <summary>
        /// 1020
        /// </summary>
        ControllerCalc = 1020,
        /// <summary>
        /// 1030
        /// </summary>
        ControllerStatus = 1030,

        /// <summary>
        /// 2000
        /// </summary>
        Render = 2000,
        /// <summary>
        /// 2010
        /// </summary>
        RenderPick = 2010,
        /// <summary>
        /// 2020
        /// </summary>
        RenderAvatar = 2020,
        /// <summary>
        /// 2030
        /// </summary>
        RenderPrim = 2030,
        /// <summary>
        /// 2040
        /// </summary>
        RenderTerrain = 2040,
        /// <summary>
        /// 2050
        /// </summary>
        RenderSea = 2050,
        /// <summary>
        /// 2060
        /// </summary>
        RenderSky = 2060,
        /// <summary>
        /// 2070
        /// </summary>
        RenderCamera = 2070,
        /// <summary>
        /// 2080
        /// </summary>
        RenderFade = 2080,

        /// <summary>
        /// 9999
        /// </summary>
        Max = 9999,
    }
}
