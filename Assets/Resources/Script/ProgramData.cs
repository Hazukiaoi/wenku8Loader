using System.Collections;
using programType;

public class ProgramData {
	private ProgramData(){}
	public static readonly ProgramData instance = new ProgramData();

	pageType nowPage = pageType.TheNew;

	public pageType NowPage{
		get{
			return nowPage;
		}
		set{
			nowPage = value;
		}
	}
}
