
var PageContent = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return { 
			gridData:{rows:[]},
			page:1,
			category:null,
			gb_category:[],
			initPathName:gb_approot+'News/aj_Init'
		};  
	},		
	componentDidMount:function(){
		this.queryData(this.state.page);
		this.getAjaxInitData();//載入init資料
		return;
	},
	getAjaxInitData:function(){
		jqGet(this.state.initPathName)
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({gb_category:data.options_category});
			//載入下拉是選單內容
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	queryData:function(page){
		jqGet(this.props.dataUrl,{page:page,category:this.state.category})
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({gridData:data,page:page});
			$('html, body').scrollTop(0);
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	queryDataC:function(category){
		jqGet(this.props.dataUrl,{page:1,category:category})
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({gridData:data,page:1,category:category});
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
	categoryChange:function(event){
		this.queryDataC(event.target.value);
	},
	render:function(){

		var dpage = [];

		for(var i=1;i<=this.state.gridData.total;i++){
			dpage.push(<option value={i} >第{i}頁</option>);
		}

		var category=this.state.category;

		var outHtml = (
		<div>
			<Breadcrumb />
			<div id="content">
				<section className="wrap">
					<header className="title">
						<h2 className="float-l">最新消息</h2>
						<div className="select">
							<label for="">选择分类</label>
							<select onChange={this.categoryChange}>
								<option value="">全部</option>
								{
									this.state.gb_category.map(function(itemData,i) {
											var subOutHtml =<option key={i} value={itemData.val}>{itemData.Lname}</option>;
										return subOutHtml;
									}.bind(this))
								}
							</select>
						</div>
					</header>

					<section className="article-list">
						{
							this.state.gridData.rows.map(function(itemData,i) {
								var subOutHtml =
								<article className="pic-article" key={i}>
									<a href={this.props.contextUrl + itemData.news_id} className="thumb"><img src={itemData.imgsrc}/></a>
									<header><h4>{itemData.title}</h4></header>
								    <em>{moment(itemData.date).format('YYYY-MM-DD')}</em>
								  	<p dangerouslySetInnerHTML={this.createMarkupHtml(itemData.list_intro)}></p>
									<a href={this.props.contextUrl + itemData.news_id} className="btn">查看更多</a>
								</article>;
								return subOutHtml;
							}.bind(this))
						}
					</section>

					<PageWWW
						lang={1}
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
				<li><a href="Index">首页</a></li>
				<li>最新消息</li>
			</ul>
		);

		return outHtml;
	}
});

var comp = React.render(<PageContent dataUrl={gb_approot + 'api/GetAction/GetNewsWWW'} contextUrl={gb_approot + 'News/Content?id='} />,document.getElementById('PageContent'));