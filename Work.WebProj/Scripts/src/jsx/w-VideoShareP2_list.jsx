
var PageContent = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return { 
			gridData:{rows:[]},
			page:1
		};  
	},		
	componentDidMount:function(){
		this.queryData(this.state.page);
		return;
	},
	queryData:function(page){
		jqGet(this.props.dataUrl,{page:page})
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({gridData:data,page:page});
			$('html, body').scrollTop(0);
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	pageSelect:function(e){
		this.queryData(e.target.value);
		return false;
	},
	pageClick:function(page){
		this.queryData(page);
		return false;
	},
	pageFirst:function(){
		this.queryData(1);
		return false;
	},
	pageLast:function(){
		this.queryData(this.state.gridData.total);
		return false;
	},		
	pagePrve:function(){
		if(this.state.page > 1){
			this.queryData(this.state.page - 1);
		}else{
			this.queryData(1);
		}
		return false;
	},
	pageNext:function(){
		if(this.state.page + 1 < this.state.gridData.total){
			this.queryData(this.state.page + 1);
		}else{
			this.queryData(this.state.gridData.total);
		}
		return false;
	},
	createMarkupHtml:function(content){
		if(content!=undefined){
			content=content.replace(replace_br,'<br />');
			return {__html: content };
		}else{
			return {__html: "" };			
		}
	},
	getVieoUrl:function(url){
		url=url.substring(url.lastIndexOf("/")+1,url.length);
		return url;
	},
	render:function(){

		var dpage = [];

		for(var i=1;i<=this.state.gridData.total;i++){
			dpage.push(<option value={i} >第{i}頁</option>);
		}

		var category=this.state.category;

		var outHtml = (
		<div id="content">

			<section className="wrap">

				<header className="title">
					<h2>出國影片</h2>
				</header>

				<ol className="pic-list">
				{
					this.state.gridData.rows.map(function(itemData,i) {
						var subOutHtml =
						<li key={i}>
							<article>
								<a href={"http://www.youtube.com/embed/"+this.getVieoUrl(itemData.video_url)+"?autoplay=1"} className="thumb video" data-fancybox-type="iframe">
									<img src={itemData.imgsrc} alt="" />
								</a>
								<header><h3>{itemData.video_title}</h3></header>
							</article>
						</li>;
						return subOutHtml;
					}.bind(this))
				}
				</ol>
    		</section>


			<PageWWW 
				pageFirst={this.pageFirst}
				pagePrve={this.pagePrve}
				page={this.state.page}
				pageSelect={this.pageSelect}
				pageNext={this.pageNext}
				pageLast={this.pageLast}
				total={this.state.gridData.total}
				dpage={dpage}
			/>
		</div>
			);

		return outHtml;
	}
});


var comp = React.render(<PageContent dataUrl={gb_approot + 'api/GetAction/GetShareP2VideoWWW'} contextUrl={gb_approot + 'StudyA/p1_content?id='} />,document.getElementById('PageContent'));