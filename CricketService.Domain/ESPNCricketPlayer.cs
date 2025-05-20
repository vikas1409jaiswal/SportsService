namespace CricketService.Domain
{
    public class ESPNCricketPlayer<T>
    {
        public Player Player { get; set; }

        public Content<T> Content { get; set; }
    }

    public class Player
    {
        public string ImageUrl { get; set; }

        public string LongName { get; set; }

        public string FullName { get; set; }

        public HeadShotImage HeadshotImage { get; set; }

        public DateOfEvent DateOfBirth { get; set; } = new DateOfEvent(1000, 1, 1);

        public string PlaceOfBirth { get; set; }

        public string IntlCareerSpan { get; set; }

        public DateOfEvent? DateOfDeath { get; set; } = new DateOfEvent(1000, 1, 1);

        public string[] LongBattingStyles { get; set; }

        public string[] LongBowlingStyles { get; set; }

        public string[] PlayingRoles { get; set; }
    }

    public class Content<T>
    {
        public Profiles Profile { get; set; }

        public Summary<T> Summary { get; set; }
    }

    public class DateOfEvent
    {
        public DateOfEvent(
            int year,
            int? date = null,
            int? month = null)
        {
            Year = year;
            Month = month;
            Date = date;
        }

        public int Year { get; set; }

        public int? Month { get; set; }

        public int? Date { get; set; }
    }

    public class HeadShotImage
    {
        public string Url { get; set;}
    }

    public class Profiles
    {
        public Item[] Items { get; set; }
    }

    public class Summary<T>
    {
        public int RecordClassId { get; set; }

        public string Type { get; set; }

        public Group<T>[] Groups { get; set; }
    }

    public class Group<T>
    {
        public string Type { get; set; }

        public T[] Stats { get; set; }
    }

    public class BattingStats
    {
        public string? Tt { get; set; }

        public string? Sp { get; set; }

        public int? Mt { get; set; }

        public int? In { get; set; }

        public string? Pr { get; set; }

        public int? Rn { get; set; }

        public int? Fo { get; set; }

        public int? Si { get; set; }

        public int? Ft { get; set; }

        public int? Hn { get; set; }

        public int? Bf { get; set; }

        public int? Dk { get; set; }

        public int? No { get; set; }

        public string? Hs { get; set; }

        public double? Bta { get; set; }

        public double? Btsr { get; set; }
    }

    public class BowlingStats
    {
        public object? Bl { get; set; }

        public string? Tt { get; set; }

        public string? Sp { get; set; }

        public int? Mt { get; set; }

        public int? In { get; set; }

        public object? Pr { get; set; }

        public double? Ov { get; set; }

        public int? Cd { get; set; }

        public int? Md { get; set; }

        public int? Wk { get; set; }

        public int? Fwk { get; set; }

        public int? Fw { get; set; }

        public int? Tw { get; set; }

        public string? Bbi { get; set; }

        public string? Bbm { get; set; }

        public double? Bwa { get; set; }

        public double? Bwe { get; set; }

        public double? Bwsr { get; set; }
    }

    public class FieldingStats
    {
        public string? Tt { get; set; }

        public string? Sp { get; set; }

        public int? Mt { get; set; }

        public int? In { get; set; }

        public object? Pr { get; set; }

        public int? Ds { get; set; }

        public int? Ct { get; set; }

        public int? St { get; set; }

        public int? Ck { get; set; }

        public int? Cf { get; set; }

        public string? Mds { get; set; }

        public double? Dspi { get; set; }
    }

    public class AllRoundStats
    {
        public string? Tt { get; set; }

        public string? Sp { get; set; }

        public int? Mt { get; set; }

        public object? Pr { get; set; }

        public int? Rn { get; set; }

        public int? Wk { get; set; }

        public int? Ct { get; set; }

        public int? St { get; set; }

        public string? Hs { get; set; }

        public int? Hn { get; set; }

        public string? Bbi { get; set; }

        public double? Bta { get; set; }

        public double? Bwa { get; set; }

        public int? Fw { get; set; }

        public double? Bbad { get; set; }
    }

    public class Item
    {
        public string Type { get; set; }

        public string Html { get; set; }
    }

    //public class BackgroundImage
    //{
    //    public int Id { get; set; }

    //    public int ObjectId { get; set; }

    //    public string Slug { get; set; }

    //    public string Url { get; set; }

    //    public int Width { get; set; }

    //    public int Height { get; set; }

    //    public string Caption { get; set; }

    //    public string LongCaption { get; set; }

    //    public string Credit { get; set; }

    //    public object Photographer { get; set; }

    //    public PeerUrls PeerUrls { get; set; }
    //}

    //public class CapturedOn
    //{
    //    public int Year { get; set; }

    //    public int Month { get; set; }

    //    public int Date { get; set; }
    //}

    //public class CareerAverages
    //{
    //    public List<BattingStats> Stats { get; set; }
    //}

    //public class Content
    //{
    //    public List<PlayerTeam> Teams { get; set; }

    //    public List<object> Relations { get; set; }

    //    public Profile Profile { get; set; }

    //    public object Notes { get; set; }

    //    public object Nutshell { get; set; }

    //    public CareerAverages CareerAverages { get; set; }

    //    public List<object> TopRecords { get; set; }

    //    public Matches Matches { get; set; }

    //    public List<Story> Stories { get; set; }

    //    public List<Video> Videos { get; set; }

    //    public List<Image> Images { get; set; }

    //    public SupportInfo SupportInfo { get; set; }
    //}

    //public class Country
    //{
    //    public int Id { get; set; }

    //    public int ObjectId { get; set; }

    //    public string Name { get; set; }

    //    public string ShortName { get; set; }

    //    public string Abbreviation { get; set; }

    //    public string Slug { get; set; }

    //    public Image Image { get; set; }
    //}

    //public class CountryHighlightsUrl
    //{
    //}

    //public class CountryLiveStreamUrl
    //{
    //}

    //public class DateOfBirth
    //{
    //    public int Year { get; set; }

    //    public int Month { get; set; }

    //    public int Date { get; set; }
    //}

    //public class Event
    //{
    //    public string Interval { get; set; }

    //    public Match Match { get; set; }

    //    public object Span { get; set; }
    //}

    //public class Ground
    //{
    //    public int Id { get; set; }

    //    public int ObjectId { get; set; }

    //    public string Name { get; set; }

    //    public string SmallName { get; set; }

    //    public string LongName { get; set; }

    //    public string Slug { get; set; }

    //    public string Location { get; set; }

    //    public Image Image { get; set; }

    //    public Town Town { get; set; }

    //    public Country Country { get; set; }
    //}

    //public class HeadshotImage
    //{
    //    public int Id { get; set; }

    //    public int ObjectId { get; set; }

    //    public string Slug { get; set; }

    //    public string Url { get; set; }

    //    public int Width { get; set; }

    //    public int Height { get; set; }

    //    public string Caption { get; set; }

    //    public string LongCaption { get; set; }

    //    public object Credit { get; set; }

    //    public object Photographer { get; set; }

    //    public PeerUrls PeerUrls { get; set; }
    //}

    //public class Image
    //{
    //    public int Id { get; set; }

    //    public int ObjectId { get; set; }

    //    public string Slug { get; set; }

    //    public string Url { get; set; }

    //    public int Width { get; set; }

    //    public int Height { get; set; }

    //    public string Caption { get; set; }

    //    public string LongCaption { get; set; }

    //    public string Credit { get; set; }

    //    public object Photographer { get; set; }

    //    public object PeerUrls { get; set; }
    //}

    //public class Image9
    //{
    //    public int Id { get; set; }

    //    public int ObjectId { get; set; }

    //    public string Slug { get; set; }

    //    public string Url { get; set; }

    //    public int Width { get; set; }

    //    public int Height { get; set; }

    //    public string Caption { get; set; }

    //    public string LongCaption { get; set; }

    //    public string Credit { get; set; }

    //    public object Photographer { get; set; }

    //    public object PeerUrls { get; set; }

    //    public DateTime DateTaken { get; set; }

    //    public CapturedOn CapturedOn { get; set; }
    //}

    //public class Item
    //{
    //    public string Type { get; set; }

    //    public string Html { get; set; }
    //}

    //public class Match
    //{
    //    public MatchClass MatchClass { get; set; }

    //    public int Total { get; set; }

    //    public List<Event> Events { get; set; }

    //    public List<Type> Types { get; set; }
    //}

    //public class Match2
    //{
    //    public int Uid { get; set; }

    //    public int Id { get; set; }

    //    public int ObjectId { get; set; }

    //    public int ScribeId { get; set; }

    //    public string Slug { get; set; }

    //    public string Stage { get; set; }

    //    public string State { get; set; }

    //    public int? InternationalClassId { get; set; }

    //    public int GeneralClassId { get; set; }

    //    public object SubClassId { get; set; }

    //    public string Season { get; set; }

    //    public string Title { get; set; }

    //    public string Floodlit { get; set; }

    //    public DateTime StartDate { get; set; }

    //    public DateTime EndDate { get; set; }

    //    public DateTime StartTime { get; set; }

    //    public bool TimePublished { get; set; }

    //    public string ScheduleNote { get; set; }

    //    public bool IsCancelled { get; set; }

    //    public string Coverage { get; set; }

    //    public string CoverageNote { get; set; }

    //    public object LiveStreamUrl { get; set; }

    //    public CountryLiveStreamUrl CountryLiveStreamUrl { get; set; }

    //    public object HighlightsUrl { get; set; }

    //    public CountryHighlightsUrl CountryHighlightsUrl { get; set; }

    //    public string Status { get; set; }

    //    public string StatusText { get; set; }

    //    public string StatusEng { get; set; }

    //    public string InternationalNumber { get; set; }

    //    public object GeneralNumber { get; set; }

    //    public int? WinnerTeamId { get; set; }

    //    public int TossWinnerTeamId { get; set; }

    //    public int TossWinnerChoice { get; set; }

    //    public int ResultStatus { get; set; }

    //    public int LiveInning { get; set; }

    //    public object LiveInningPredictions { get; set; }

    //    public double? LiveOvers { get; set; }

    //    public object LiveOversPending { get; set; }

    //    public object LiveBalls { get; set; }

    //    public object LiveRecentBalls { get; set; }

    //    public object BallsPerOver { get; set; }

    //    public Series Series { get; set; }

    //    public Ground Ground { get; set; }

    //    public List<PlayerTeam> Teams { get; set; }

    //    public string DayType { get; set; }

    //    public string Format { get; set; }

    //    public int? PreviewStoryId { get; set; }

    //    public int? ReportStoryId { get; set; }

    //    public object LiveBlogStoryId { get; set; }

    //    public object FantasyPickStoryId { get; set; }

    //    public object DrawOdds { get; set; }

    //    public bool IsSuperOver { get; set; }

    //    public bool IsScheduledInningsComplete { get; set; }

    //    public int TotalGalleries { get; set; }

    //    public int TotalImages { get; set; }

    //    public int TotalVideos { get; set; }

    //    public int TotalStories { get; set; }

    //    public List<object> Languages { get; set; }

    //    public DateTime GeneratedAt { get; set; }
    //}

    //public class MatchClass
    //{
    //    public int Id { get; set; }

    //    public string Name { get; set; }

    //    public string Type { get; set; }
    //}

    //public class MatchClassMeta
    //{
    //    public int Id { get; set; }

    //    public string Type { get; set; }

    //    public string Name { get; set; }
    //}

    //public class MatchMeta
    //{
    //    public int Id { get; set; }

    //    public int ObjectId { get; set; }

    //    public int ScribeId { get; set; }

    //    public string Slug { get; set; }

    //    public int SeriesId { get; set; }

    //    public int SeriesObjectId { get; set; }

    //    public string SeriesSlug { get; set; }
    //}

    //public class Origin
    //{
    //    public string Type { get; set; }
    //}

    //public class OverallStat
    //{
    //    public int RecordClassId { get; set; }

    //    public int Matches { get; set; }

    //    public bool Batted { get; set; }

    //    public int BattedInnings { get; set; }

    //    public bool Bowled { get; set; }

    //    public int BowledInnings { get; set; }

    //    public bool Fielded { get; set; }

    //    public int FieldedInnings { get; set; }

    //    public int Dismissals { get; set; }

    //    public int Stumped { get; set; }

    //    public int UmpireMatches { get; set; }

    //    public int RefereeMatches { get; set; }
    //}

    //public class PeerUrls
    //{
    //    public object FILM { get; set; }

    //    public object WIDE { get; set; }

    //    public string SQUARE { get; set; }
    //}

    //public class Player
    //{
    //    public int Id { get; set; }

    //    public int ObjectId { get; set; }

    //    public string Name { get; set; }

    //    public string LongName { get; set; }

    //    public string MobileName { get; set; }

    //    public string IndexName { get; set; }

    //    public string BattingName { get; set; }

    //    public string FieldingName { get; set; }

    //    public string Slug { get; set; }

    //    public string ImageUrl { get; set; }

    //    public DateOfBirth DateOfBirth { get; set; }

    //    public object DateOfDeath { get; set; }

    //    public string Gender { get; set; }

    //    public List<string> BattingStyles { get; set; }

    //    public List<string> BowlingStyles { get; set; }

    //    public List<string> LongBattingStyles { get; set; }

    //    public List<string> LongBowlingStyles { get; set; }

    //    public Image Image { get; set; }

    //    public int CountryTeamId { get; set; }

    //    public List<int> PlayerRoleTypeIds { get; set; }

    //    public List<string> PlayingRoles { get; set; }

    //    public string FullName { get; set; }

    //    public string NickNames { get; set; }

    //    public string AlsoKnownAs { get; set; }

    //    public double Height { get; set; }

    //    public string HeightUnit { get; set; }

    //    public string Education { get; set; }

    //    public Country Country { get; set; }

    //    public object AltCountryTeamId { get; set; }

    //    public string PlaceOfBirth { get; set; }

    //    public object DobText { get; set; }

    //    public object DodText { get; set; }

    //    public string PlaceOfDeath { get; set; }

    //    public string IntlCareerSpan { get; set; }

    //    public HeadshotImage HeadshotImage { get; set; }

    //    public BackgroundImage BackgroundImage { get; set; }

    //    public List<object> ExternalLinks { get; set; }

    //    public List<object> FieldingStyles { get; set; }

    //    public List<object> OtherStyles { get; set; }

    //    public List<int> MatchPlayerTypes { get; set; }

    //    public bool HasRecords { get; set; }

    //    public bool HasStats { get; set; }

    //    public bool HasMatches { get; set; }

    //    public int TotalStories { get; set; }

    //    public int TotalVideos { get; set; }

    //    public int TotalImages { get; set; }
    //}

    //public class Profile
    //{
    //    public List<Item> Items { get; set; }
    //}

    //public class Recent
    //{
    //    public Match Match { get; set; }

    //    public int TeamId { get; set; }

    //    public string BattingText { get; set; }

    //    public object BowlingText { get; set; }

    //    public object KeepingText { get; set; }
    //}

    //public class RecordClassMeta
    //{
    //    public int Id { get; set; }

    //    public string Type { get; set; }

    //    public string Name { get; set; }

    //    public string LongName { get; set; }

    //    public int MultipleClass { get; set; }

    //    public string Slug { get; set; }
    //}

    //public class Root
    //{
    //    public Player Player { get; set; }

    //    public Content Content { get; set; }
    //}

    //public class Series
    //{
    //    public int Id { get; set; }

    //    public int ObjectId { get; set; }

    //    public int ScribeId { get; set; }

    //    public string Slug { get; set; }

    //    public string Name { get; set; }

    //    public string LongName { get; set; }

    //    public string AlternateName { get; set; }

    //    public string LongAlternateName { get; set; }

    //    public object UnofficialName { get; set; }

    //    public int Year { get; set; }

    //    public int TypeId { get; set; }

    //    public bool IsTrophy { get; set; }

    //    public string Description { get; set; }

    //    public string Season { get; set; }

    //    public DateTime StartDate { get; set; }

    //    public DateTime EndDate { get; set; }

    //    public bool HasStandings { get; set; }

    //    public int TotalVideos { get; set; }

    //    public bool GamePlayWatch { get; set; }
    //}

    //public class BattingStats
    //{
    //    public string Type { get; set; }

    //    public int Cl { get; set; }

    //    public int Mt { get; set; }

    //    public int In { get; set; }

    //    public int Rn { get; set; }

    //    public int Bl { get; set; }

    //    public double? Avg { get; set; }

    //    public double? Sr { get; set; }

    //    public int No { get; set; }

    //    public int Fo { get; set; }

    //    public int Si { get; set; }

    //    public string Hs { get; set; }

    //    public int Hn { get; set; }

    //    public int Ft { get; set; }

    //    public int Ct { get; set; }

    //    public int St { get; set; }

    //    public int? Wk { get; set; }

    //    public string Bbi { get; set; }

    //    public string Bbm { get; set; }

    //    public int? Fwk { get; set; }

    //    public int? Fw { get; set; }

    //    public int? Tw { get; set; }

    //    public double? Bwe { get; set; }
    //}

    //public class Story
    //{
    //    public int Id { get; set; }

    //    public int ObjectId { get; set; }

    //    public int ScribeId { get; set; }

    //    public string Slug { get; set; }

    //    public string Title { get; set; }

    //    public string SubTitle { get; set; }

    //    public string SeoTitle { get; set; }

    //    public string Summary { get; set; }

    //    public string Byline { get; set; }

    //    public string CategoryType { get; set; }

    //    public string GenreType { get; set; }

    //    public int GenreId { get; set; }

    //    public string GenreName { get; set; }

    //    public DateTime PublishedAt { get; set; }

    //    public DateTime ModifiedAt { get; set; }

    //    public bool ShowPublishedAt { get; set; }

    //    public bool ShowModifiedAt { get; set; }

    //    public object Day { get; set; }

    //    public int AuthorId { get; set; }

    //    public Image Image { get; set; }

    //    public MatchMeta MatchMeta { get; set; }

    //    public bool IsLiveBlog { get; set; }

    //    public bool IsLive { get; set; }

    //    public string Language { get; set; }

    //    public DateTime GeneratedAt { get; set; }
    //}

    //public class SupportInfo
    //{
    //    public TeamRecentTeam TeamRecentTeam { get; set; }

    //    public List<TeamRecentPlayer> TeamRecentPlayers { get; set; }

    //    public List<TeamPopularPlayer> TeamPopularPlayers { get; set; }

    //    public List<MatchClassMeta> MatchClassMetas { get; set; }

    //    public List<RecordClassMeta> RecordClassMetas { get; set; }

    //    public List<OverallStat> OverallStats { get; set; }

    //    public TeamFormatCapCounts TeamFormatCapCounts { get; set; }

    //    public int TeamContractedPlayerCount { get; set; }

    //    public List<TeamMatchClassMeta> TeamMatchClassMetas { get; set; }
    //}

    //public class PlayerTeam
    //{
    //    public PlayerTeam Team { get; set; }

    //    public bool Current { get; set; }

    //    public bool IsHome { get; set; }

    //    public bool IsLive { get; set; }

    //    public string Score { get; set; }

    //    public string ScoreInfo { get; set; }

    //    public List<int> InningNumbers { get; set; }

    //    public object Points { get; set; }

    //    public object SidePlayers { get; set; }

    //    public object SideBatsmen { get; set; }

    //    public object SideFielders { get; set; }

    //    public object Captain { get; set; }

    //    public object TeamOdds { get; set; }
    //}

    //public class Team2
    //{
    //    public int Id { get; set; }

    //    public int ObjectId { get; set; }

    //    public int ScribeId { get; set; }

    //    public string Slug { get; set; }

    //    public string Name { get; set; }

    //    public string LongName { get; set; }

    //    public string Abbreviation { get; set; }

    //    public object UnofficialName { get; set; }

    //    public string ImageUrl { get; set; }

    //    public bool IsCountry { get; set; }

    //    public string PrimaryColor { get; set; }

    //    public Image Image { get; set; }
    //}

    //public class TeamFormatCapCounts
    //{
    //    [JsonProperty("1")]
    //    public int _1 { get; set; }

    //    [JsonProperty("2")]
    //    public int _2 { get; set; }

    //    [JsonProperty("3")]
    //    public int _3 { get; set; }

    //    [JsonProperty("8")]
    //    public int _8 { get; set; }

    //    [JsonProperty("9")]
    //    public int _9 { get; set; }

    //    [JsonProperty("10")]
    //    public int _10 { get; set; }

    //    [JsonProperty("14")]
    //    public int _14 { get; set; }

    //    [JsonProperty("15")]
    //    public int _15 { get; set; }

    //    [JsonProperty("16")]
    //    public int _16 { get; set; }
    //}

    //public class TeamMatchClassMeta
    //{
    //    public int Id { get; set; }

    //    public string Type { get; set; }

    //    public string Name { get; set; }
    //}

    //public class TeamPopularPlayer
    //{
    //    public int Id { get; set; }

    //    public int ObjectId { get; set; }

    //    public string Name { get; set; }

    //    public string LongName { get; set; }

    //    public string MobileName { get; set; }

    //    public string IndexName { get; set; }

    //    public string BattingName { get; set; }

    //    public string FieldingName { get; set; }

    //    public string Slug { get; set; }

    //    public string ImageUrl { get; set; }

    //    public DateOfBirth DateOfBirth { get; set; }

    //    public object DateOfDeath { get; set; }

    //    public string Gender { get; set; }

    //    public List<string> BattingStyles { get; set; }

    //    public List<string> BowlingStyles { get; set; }

    //    public List<string> LongBattingStyles { get; set; }

    //    public List<string> LongBowlingStyles { get; set; }

    //    public Image Image { get; set; }

    //    public int CountryTeamId { get; set; }

    //    public List<int> PlayerRoleTypeIds { get; set; }

    //    public List<string> PlayingRoles { get; set; }
    //}

    //public class TeamRecentPlayer
    //{
    //    public int Id { get; set; }

    //    public int ObjectId { get; set; }

    //    public string Name { get; set; }

    //    public string LongName { get; set; }

    //    public string MobileName { get; set; }

    //    public string IndexName { get; set; }

    //    public string BattingName { get; set; }

    //    public string FieldingName { get; set; }

    //    public string Slug { get; set; }

    //    public string ImageUrl { get; set; }

    //    public DateOfBirth DateOfBirth { get; set; }

    //    public object DateOfDeath { get; set; }

    //    public string Gender { get; set; }

    //    public List<string> BattingStyles { get; set; }

    //    public List<string> BowlingStyles { get; set; }

    //    public List<string> LongBattingStyles { get; set; }

    //    public List<string> LongBowlingStyles { get; set; }

    //    public Image Image { get; set; }

    //    public int CountryTeamId { get; set; }

    //    public List<int> PlayerRoleTypeIds { get; set; }

    //    public List<string> PlayingRoles { get; set; }
    //}

    //public class TeamRecentTeam
    //{
    //    public int Id { get; set; }

    //    public int ObjectId { get; set; }

    //    public int ScribeId { get; set; }

    //    public string Slug { get; set; }

    //    public string Name { get; set; }

    //    public string LongName { get; set; }

    //    public string Abbreviation { get; set; }

    //    public object UnofficialName { get; set; }

    //    public string ImageUrl { get; set; }

    //    public bool IsCountry { get; set; }

    //    public string PrimaryColor { get; set; }

    //    public Image Image { get; set; }
    //}

    //public class Town
    //{
    //    public int Id { get; set; }

    //    public int ObjectId { get; set; }

    //    public string Name { get; set; }

    //    public string Area { get; set; }

    //    public string Timezone { get; set; }
    //}

    //public class Type
    //{
    //    public int Type { get; set; }

    //    public List<Match> Matches { get; set; }

    //    public List<Recent> Recent { get; set; }
    //}

    //public class Video
    //{
    //    public int Id { get; set; }

    //    public int ObjectId { get; set; }

    //    public int ScribeId { get; set; }

    //    public int StatusTypeId { get; set; }

    //    public string Slug { get; set; }

    //    public string Title { get; set; }

    //    public string SubTitle { get; set; }

    //    public string SeoTitle { get; set; }

    //    public string Summary { get; set; }

    //    public int Duration { get; set; }

    //    public int GenreId { get; set; }

    //    public string GenreType { get; set; }

    //    public string GenreName { get; set; }

    //    public DateTime PublishedAt { get; set; }

    //    public DateTime ModifiedAt { get; set; }

    //    public DateTime RecordedAt { get; set; }

    //    public DateTime ExpireAt { get; set; }

    //    public List<object> CountryCodes { get; set; }

    //    public string ImageUrl { get; set; }

    //    public Origin Origin { get; set; }

    //    public object AssetId { get; set; }

    //    public string ScribeMongoId { get; set; }

    //    public string Language { get; set; }
    //}
}
