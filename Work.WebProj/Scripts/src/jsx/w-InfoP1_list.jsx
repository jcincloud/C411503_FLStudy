
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
	render:function(){

		var dpage = [];

		for(var i=1;i<=this.state.gridData.total;i++){
			dpage.push(<option value={i} >第{i}頁</option>);
		}

		var category=this.state.category;

		var outHtml = (
		<div>
			<div id="content">
				<section className="wrap">
					<header className="title">
						<h2 className="float-l">遊留學百科</h2>
					</header>

					<section className="article-list">
						{
							this.state.gridData.rows.map(function(itemData,i) {
								var subOutHtml =
								<article className="pic-article" key={i}>
									<a href={this.props.contextUrl + itemData.helpful_info_id} className="thumb"><img src={itemData.imgsrc}/></a>
									<header><h4>{itemData.title}</h4></header>
								  	<p dangerouslySetInnerHTML={this.createMarkupHtml(itemData.list_intro)}></p>
									<a href={this.props.contextUrl + itemData.helpful_info_id} className="btn">查看更多</a>
								</article>;
								return subOutHtml;
							}.bind(this))
						}
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
				</section>
			</div>
		</div>
			);

		return outHtml;
	}
});

//麵包屑元件
var Breadcrumb = React.createClass({
	render:function(){
		var outHtml = (
				<ul className="breadcrumb">
					<li><a href={gb_approot}>首頁</a></li>
					<li><a href={gb_approot+'Info'}>遊學/留學小幫手</a></li>
					<li>遊留學百科</li>
				</ul>
		);

		return outHtml;
	}
});

var comp = React.render(<PageContent dataUrl={gb_approot + 'api/GetAction/GetInfoP1WWW'} contextUrl={gb_approot + 'Info/p1_content?id='} />,document.getElementById('PageContent'));