namespace UI
{
    public interface IMenuSlider
    {
        /// <summary>
        /// Slides the menus to the proper place 
        /// 0 = home, 1 = profile, etc.. see MenuView enum
        /// </summary>
        /// <param name="to"></param>
        void SlideMenuTo(int to);
    }
}