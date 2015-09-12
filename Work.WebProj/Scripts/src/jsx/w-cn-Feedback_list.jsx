
var PageContent = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return { 
			gridData:{rows:[]},
			page:1,
			category:null,
			year:null,
			gb_category:[],
			gb_year:[],
			initPathName:gb_approot+'Share/aj_Init'
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
			this.setState({gb_category:data.options_category,gb_year:data.options_year});
			//載入下拉是選單內容
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	queryData:function(page){
		jqGet(this.props.dataUrl,{page:page,category:this.state.category,year:this.state.year})
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({gridData:data,page:page});
			$('html, body').scrollTop(0);
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	queryDataC:function(category){
		jqGet(this.props.dataUrl,{page:1,category:category,year:this.state.year})
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({gridData:data,page:1,category:category});
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	queryDataY:function(year){
		jqGet(this.props.dataUrl,{page:1,category:this.state.category,year:year})
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({gridData:data,page:1,year:year});
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
	categoryYChange:function(event){
		this.queryDataY(event.target.value);
	},
	render:function(){

		var dpage = [];

		for(var i=1;i<=this.state.gridData.total;i++){
			dpage.push(<option value={i} >第{i}页</option>);
		}

		var outHtml = (
		<div>
				<section className="wrap">
					<header className="title">
						<h2 className="float-l">心得分享</h2>
						<div className="select">
							<label for="">选择年度</label>
							<select onChange={this.categoryYChange}>
								<option value="">全部</option>
								{
									this.state.gb_year.map(function(itemData,i) {
											var subOutHtml =<option key={i} value={itemData.val}>{itemData.Lname}</option>;
										return subOutHtml;
									}.bind(this))
								}
							</select>
							<br />
							<label for="">选择类别</label>
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

					{/*<section className="quotes flexslider">
    					<header className="hidden"><h3>聽聽我們的學員和家長怎麼說</h3></header>
    					<ul className="slides">
    					{
							this.state.gridData.rows.map(function(itemData,i) {
								var subOutHtmlforSlides =
    						<li key={i}>
    							<article>
    								<blockquote><a href={this.props.contextUrl + itemData.feedback_id}>{itemData.feedback_sort}</a></blockquote>
    								<header><h4>{itemData.title}</h4></header>
    							</article>
    						</li>;
								return subOutHtmlforSlides;
							}.bind(this))
						}
    					</ul>
    	    		</section>*/}
    					<ol className="pic-list">
    					{
							this.state.gridData.rows.map(function(itemData,i) {
								var subOutHtml =
    						<li key={i}>
                				<article>
        							<a href={this.props.contextUrl + itemData.feedback_id} className="thumb">
        							<img src={itemData.imgsrc} alt="" />
        							<span>
    	    							<em>{itemData.feedback_content}</em>
    	    							<i>READ MORE</i>
        							</span>
        							</a>
        							<header><h3>{itemData.title}</h3></header>
                				</article>
    						</li>;
								return subOutHtml;
							}.bind(this))
						}
    					</ol>


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
			);

		return outHtml;
	}
});

//麵包屑元件
var Breadcrumb = React.createClass({
	render:function(){
		var outHtml = (
			<ul className="breadcrumb">
				<li><a href="Index">首頁</a></li>
				<li>最新消息</li>
			</ul>
		);

		return outHtml;
	}
});

var comp = React.render(<PageContent dataUrl={gb_approot + 'api/GetAction/GetFeedbackWWW'} contextUrl={gb_approot + 'Share/p1_content?id='} />,document.getElementById('PageContent'));