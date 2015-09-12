var SectionText = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return { 
		};  
	},
	render:function(){

		var outHtml = (
        	<section id={this.props.sectionId}>
            	<h4>LATEST NEWS</h4>
            	<ul>
                	<li><a href="~/News/News_content"><em>2010/12/31</em>加入中壢青商＝投資您的未來！！加入中壢青商＝投資您的未來！！加入中壢青商＝投資您的未來！！加入中壢青商＝投資您的未來！！加入中壢青商＝投資您的未來！！加入中壢青商＝投資您的未來！！</a></li>
                	<li><a href="~/News/News_content"><em>2011/3/28</em>中壢國際青年商會的FaceBook中壢國際青年商會的FaceBook中壢國際青年商會的FaceBook</a></li>
                	<li><a href="~/News/News_content"><em>2011/12/28</em>101年度各項事務申請表格101年度各項事務申請表格101年度各項事務申請表格</a></li>
                	<li><a href="~/News/News_content"><em>2014/10/3</em>第四十一屆第四次理監事預備會第四十一屆第四次理監事預備會第四十一屆第四次理監事預備</a></li>
                	<li><a href="~/News/News_content"><em>2014/9/12</em>特友會老中青聯誼暨秋季月光晚會特友會老中青聯誼暨秋季月光晚會特友會老中青聯誼暨秋季</a></li>
                	<li><a href="~/News/News_content"><em>2014/9/28</em>九月份秋季聯歡暨月例會九月份秋季聯歡暨月例會九月份秋季聯歡暨月例會九月份</a></li>
                	<li><a href="~/News/News_content"><em>2014/9/10</em>第四十一屆第九次理事會第四十一屆第九次理事會第四十一屆第九次理事會</a></li>
                	<li><a href="~/News/News_content"><em>2014/10/6</em>第四十一屆第十次理事會</a></li>
            	</ul>
        	</section>
			);

		return outHtml;
	}
});


var tabTexts = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return { 
		};  
	},
	render:function(){

		var outHtml = (
			<span>
				<SectionText sectionId="sec1" />
				<SectionText sectionId="sec2" />
				<SectionText sectionId="sec3" />
				<SectionText sectionId="sec4" />
			</span>
			);

		return outHtml;
	}
});
var getRootDOM = document.getElementById('tabContent');
var Comp = React.renderComponent(<tabTexts />,getRootDOM);