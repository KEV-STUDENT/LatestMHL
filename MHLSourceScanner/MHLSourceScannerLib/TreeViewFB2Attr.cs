using MHLCommon.MHLBook;
using MHLSourceScannerModelLib;
using System.Windows;
using System.Windows.Media;

namespace MHLSourceScannerLib
{
    public struct Decor4FB2Attr : IDecorator
    {
        Brush IDecorator.ForeGround => Brushes.Black;
        FontWeight IDecorator.FontWeight => FontWeights.Normal;
        bool IDecorator.Focusable => false;
    }

    public class FB2Genre : TreeViewFB2Attr<Decor4FB2Attr, MHLGenre>
    {
        #region [Constructors]
        public FB2Genre(MHLGenre bookAttribute) : base(bookAttribute)
        {
            switch (bookAttribute.Genre)
            {
                #region Фантастика (Научная фантастика и Фэнтези)
                case MHLCommon.FB2Genres.sf_history:
                    Name = "Альтернативная история";
                    break;
                case MHLCommon.FB2Genres.sf_action:
                    Name = "Боевая фантастика";
                    break;
                case MHLCommon.FB2Genres.sf_epic:
                    Name = "Эпическая фантастика";
                    break;
                case MHLCommon.FB2Genres.sf_heroic:
                    Name = "Героическая фантастика";
                    break;
                case MHLCommon.FB2Genres.sf_detective:
                    Name = " Детективная фантастика";
                    break;
                case MHLCommon.FB2Genres.sf_cyberpunk:
                    Name = "Киберпанк";
                    break;
                case MHLCommon.FB2Genres.sf_space:
                    Name = "Космическая фантастика";
                    break;
                case MHLCommon.FB2Genres.sf_social:
                    Name = "Социально-психологическая фантастика";
                    break;
                case MHLCommon.FB2Genres.sf_horror:
                    Name = "Ужасы и Мистика";
                    break;
                case MHLCommon.FB2Genres.sf_humor:
                    Name = "Юмористическая фантастика";
                    break;
                case MHLCommon.FB2Genres.sf_fantasy:
                    Name = "Фэнтези";
                    break;
                case MHLCommon.FB2Genres.sf:
                    Name = "Научная Фантастика";
                    break;
                #endregion

                #region Детективы и Триллеры
                case MHLCommon.FB2Genres.det_classic:
                    Name = "Классический детектив";
                    break;
                case MHLCommon.FB2Genres.det_police:
                    Name = "Полицейский детектив";
                    break;
                case MHLCommon.FB2Genres.det_action:
                    Name = "Боевик";
                    break;
                case MHLCommon.FB2Genres.det_irony:
                    Name = "Иронический детектив";
                    break;
                case MHLCommon.FB2Genres.det_history:
                    Name = "Исторический детектив";
                    break;
                case MHLCommon.FB2Genres.det_espionage:
                    Name = "Шпионский детектив";
                    break;
                case MHLCommon.FB2Genres.det_crime:
                    Name = "Криминальный детектив";
                    break;
                case MHLCommon.FB2Genres.det_political:
                    Name = "Политический детектив";
                    break;
                case MHLCommon.FB2Genres.det_maniac:
                    Name = "Маньяки";
                    break;
                case MHLCommon.FB2Genres.det_hard:
                    Name = "Крутой детектив";
                    break;
                case MHLCommon.FB2Genres.thriller:
                    Name = "Триллер";
                    break;
                case MHLCommon.FB2Genres.detective:
                    Name = "Детектив (не относящийся в прочие категории).";
                    break;
                #endregion

                #region Проза
                case MHLCommon.FB2Genres.prose_classic:
                    Name = "Классическая проза";
                    break;
                case MHLCommon.FB2Genres.prose_history:
                    Name = "Историческая проза";
                    break;
                case MHLCommon.FB2Genres.prose_contemporary:
                    Name = "Современная проза";
                    break;
                case MHLCommon.FB2Genres.prose_counter:
                    Name = "Контркультура";
                    break;
                case MHLCommon.FB2Genres.prose_rus_classic:
                    Name = "Русская классическая проза";
                    break;
                case MHLCommon.FB2Genres.prose_su_classics:
                    Name = "Советская классическая проза";
                    break;
                #endregion

                #region Любовные романы
                case MHLCommon.FB2Genres.love_contemporary:
                    Name = "Современные любовные романы";
                    break;
                case MHLCommon.FB2Genres.love_history:
                    Name = "Исторические любовные романы";
                    break;
                case MHLCommon.FB2Genres.love_detective:
                    Name = "Остросюжетные любовные романы";
                    break;
                case MHLCommon.FB2Genres.love_short:
                    Name = "Короткие любовные романы";
                    break;
                case MHLCommon.FB2Genres.love_erotica:
                    Name = "Эротика";
                    break;
                #endregion

                #region Приключения
                case MHLCommon.FB2Genres.adv_western:
                    Name = "Вестерн";
                    break;
                case MHLCommon.FB2Genres.adv_history:
                    Name = "Исторические приключения";
                    break;
                case MHLCommon.FB2Genres.adv_indian:
                    Name = "Приключения про индейцев";
                    break;
                case MHLCommon.FB2Genres.adv_maritime:
                    Name = "Морские приключения";
                    break;
                case MHLCommon.FB2Genres.adv_geo:
                    Name = "Путешествия и география";
                    break;
                case MHLCommon.FB2Genres.adv_animal:
                    Name = "Природа и животные";
                    break;
                case MHLCommon.FB2Genres.adventure:
                    Name = "Приключения";
                    break;
                #endregion

                #region Детское
                case MHLCommon.FB2Genres.child_tale:
                    Name = "Сказка";
                    break;
                case MHLCommon.FB2Genres.child_verse:
                    Name = "Детские стихи";
                    break;
                case MHLCommon.FB2Genres.child_prose:
                    Name = "Детскиая проза";
                    break;
                case MHLCommon.FB2Genres.child_sf:
                    Name = "Детская фантастика";
                    break;
                case MHLCommon.FB2Genres.child_det:
                    Name = "Детские остросюжетные";
                    break;
                case MHLCommon.FB2Genres.child_adv:
                    Name = "Детские приключения";
                    break;
                case MHLCommon.FB2Genres.child_education:
                    Name = "Детская образовательная литература";
                    break;
                case MHLCommon.FB2Genres.children:
                    Name = "Детская литература";
                    break;
                #endregion

                #region Поэзия, Драматургия
                case MHLCommon.FB2Genres.poetry:
                    Name = "Поэзия";
                    break;
                case MHLCommon.FB2Genres.dramaturgy:
                    Name = "Драматургия";
                    break;
                #endregion

                #region Старинное
                case MHLCommon.FB2Genres.antique_ant:
                    Name = "Античная литература";
                    break;
                case MHLCommon.FB2Genres.antique_european:
                    Name = "Европейская старинная литература";
                    break;
                case MHLCommon.FB2Genres.antique_russian:
                    Name = "Древнерусская литература";
                    break;
                case MHLCommon.FB2Genres.antique_east:
                    Name = "Древневосточная литература";
                    break;
                case MHLCommon.FB2Genres.antique_myths:
                    Name = "Мифы. Легенды. Эпос";
                    break;
                case MHLCommon.FB2Genres.antique:
                    Name = "Прочая старинная литература";
                    break;
                #endregion

                #region Наука, Образование
                case MHLCommon.FB2Genres.sci_history:
                    Name = "История";
                    break;
                case MHLCommon.FB2Genres.sci_psychology:
                    Name = "Психология";
                    break;
                case MHLCommon.FB2Genres.sci_culture:
                    Name = "Культурология";
                    break;
                case MHLCommon.FB2Genres.sci_religion:
                    Name = "Религиоведение";
                    break;
                case MHLCommon.FB2Genres.sci_philosophy:
                    Name = "Философия";
                    break;
                case MHLCommon.FB2Genres.sci_politics:
                    Name = "Политика";
                    break;
                case MHLCommon.FB2Genres.sci_business:
                    Name = "Деловая литература";
                    break;
                case MHLCommon.FB2Genres.sci_juris:
                    Name = "Юриспруденция";
                    break;
                case MHLCommon.FB2Genres.sci_linguistic:
                    Name = "Языкознание";
                    break;
                case MHLCommon.FB2Genres.sci_medicine:
                    Name = "Медицина";
                    break;
                case MHLCommon.FB2Genres.sci_phys:
                    Name = "Физика";
                    break;
                case MHLCommon.FB2Genres.sci_math:
                    Name = "Математика";
                    break;
                case MHLCommon.FB2Genres.sci_chem:
                    Name = "Химия";
                    break;
                case MHLCommon.FB2Genres.sci_biology:
                    Name = "Биология";
                    break;
                case MHLCommon.FB2Genres.sci_tech:
                    Name = "Технические науки";
                    break;
                case MHLCommon.FB2Genres.science:
                    Name = "Научная литература";
                    break;
                #endregion

                #region Компьютеры и Интернет
                case MHLCommon.FB2Genres.comp_www:
                    Name = "Интернет";
                    break;
                case MHLCommon.FB2Genres.comp_programming:
                    Name = "Программирование";
                    break;
                case MHLCommon.FB2Genres.comp_hard:
                    Name = "Компьютерное железо (аппаратное обеспечение)";
                    break;
                case MHLCommon.FB2Genres.comp_soft:
                    Name = "Программы";
                    break;
                case MHLCommon.FB2Genres.comp_db:
                    Name = "Базы данных";
                    break;
                case MHLCommon.FB2Genres.comp_osnet:
                    Name = "ОС и Сети";
                    break;
                case MHLCommon.FB2Genres.computers:
                    Name = "Околокомпьтерная литература";
                    break;
                #endregion

                #region Справочная литература
                case MHLCommon.FB2Genres.ref_encyc:
                    Name = "Энциклопедии";
                    break;
                case MHLCommon.FB2Genres.ref_dict:
                    Name = "Словари";
                    break;
                case MHLCommon.FB2Genres.ref_ref:
                    Name = "Справочники";
                    break;
                case MHLCommon.FB2Genres.ref_guide:
                    Name = "Руководства";
                    break;
                case MHLCommon.FB2Genres.reference:
                    Name = "Справочная литература";
                    break;
                #endregion

                #region Документальная литература
                case MHLCommon.FB2Genres.nonf_biography:
                    Name = "Биографии и Мемуары";
                    break;
                case MHLCommon.FB2Genres.nonf_publicism:
                    Name = "Публицистика";
                    break;
                case MHLCommon.FB2Genres.nonf_criticism:
                    Name = "Критика";
                    break;
                design: Name = "Искусство и Дизайн";
                    break;
                case MHLCommon.FB2Genres.nonfiction:
                    Name = "Документальная литература";
                    break;
                #endregion

                #region Религия и духовность
                case MHLCommon.FB2Genres.religion_rel:
                    Name = "Религия";
                    break;
                case MHLCommon.FB2Genres.religion_esoterics:
                    Name = "Эзотерика";
                    break;
                case MHLCommon.FB2Genres.religion_self:
                    Name = "Самосовершенствование";
                    break;
                case MHLCommon.FB2Genres.religion:
                    Name = "Религиозная литература";
                    break;
                #endregion

                #region Юмор
                case MHLCommon.FB2Genres.humor_anecdote:
                    Name = "Анекдоты";
                    break;
                case MHLCommon.FB2Genres.humor_prose:
                    Name = "Юмористическая проза";
                    break;
                case MHLCommon.FB2Genres.humor_verse:
                    Name = "Юмористические стихи";
                    break;
                case MHLCommon.FB2Genres.humor:
                    Name = "Юмор";
                    break;
                #endregion

                #region Домоводство (Дом и семья)
                case MHLCommon.FB2Genres.home_cooking:
                    Name = "Кулинария";
                    break;
                case MHLCommon.FB2Genres.home_pets:
                    Name = "Домашние животные";
                    break;
                case MHLCommon.FB2Genres.home_crafts:
                    Name = "Хобби и ремесла";
                    break;
                case MHLCommon.FB2Genres.home_entertain:
                    Name = "Развлечения";
                    break;
                case MHLCommon.FB2Genres.home_health:
                    Name = "Здоровье";
                    break;
                case MHLCommon.FB2Genres.home_garden:
                    Name = "Сад и огород";
                    break;
                case MHLCommon.FB2Genres.home_diy:
                    Name = "Сделай сам";
                    break;
                case MHLCommon.FB2Genres.home_sport:
                    Name = "Спорт";
                    break;
                case MHLCommon.FB2Genres.home_sex:
                    Name = "Эротика, Секс";
                    break;
                case MHLCommon.FB2Genres.home:
                    Name = "Домоводство";
                    break;
                #endregion

                #region Дополнительные
                case MHLCommon.FB2Genres.popadanec:
                    Name = "Попаданец";
                    break;
                #endregion
                default:
                    Name = bookAttribute.Genre.ToString();
                    break;
            }
        }
        #endregion
    }

    public class FB2Author : TreeViewFB2Attr<Decor4FB2Attr, MHLAuthor>
    {
        #region [Proprties]
        public string LastName
        {
            get => bookAttribute.LastName;
        }

        public string FirstName
        {
            get => bookAttribute.FirstName;
        }

        public string MiddleName
        {
            get => bookAttribute.MiddleName;
        }
        #endregion

        #region [Constructors]
        public FB2Author(MHLAuthor bookAttribute) : base(bookAttribute)
        {
            Name = string.Format("{0} {1} {2}", bookAttribute.LastName.Trim(), bookAttribute.FirstName.Trim(), bookAttribute.MiddleName.Trim()).Trim();
        }
        #endregion
    }

    public abstract class TreeViewFB2Attr<T1, T2> : TreeItem
    where T1 : IDecorator, new()
    where T2 : IBookAttribute
    {
        #region [Fields]
        private readonly T1 decorator = new T1();
        protected readonly T2 bookAttribute;
        #endregion

        #region [Constructors]
        public TreeViewFB2Attr(T2 bookAttribute)
        {
            this.bookAttribute = bookAttribute;
        }
        #endregion

        #region [Proprties]
        public Brush ForeGround
        {
            get => decorator.ForeGround;
        }

        public FontWeight FontWeight
        {
            get => decorator.FontWeight;
        }

        public bool Focusable
        {
            get => decorator.Focusable;
        }
        #endregion
    }
}
