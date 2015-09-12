
var PageContent = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return { 
			gridData:{rows:[]},
			page:1,
			category:0,
			country:null,
			vacationCategory:[],
			countryCategory:[],
			initPathName:gb_approot+'StudyTravel/aj_Init'
		};  
	},		
	componentDidMount:function(){
		this.queryData(this.state.page);
		this.getAjaxInitData();//載入init資料
		this.setState({category:gb_category});
		return;
	},
	getAjaxInitData:function(){
		jqGet(this.state.initPathName)
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({vacationCategory:data.options_vacation_category,countryCategory:data.options_country_category});
			//載入下拉是選單內容
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	queryData:function(page){
		jqGet(this.props.dataUrl,{page:page,country:this.state.country,category:gb_category})
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({gridData:data,page:page});
			$('html, body').scrollTop(0);
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	queryDataC:function(country){
		jqGet(this.props.dataUrl,{page:1,country:country,category:gb_category})
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({gridData:data,page:1,country:country});
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
	countryChange:function(event){
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
			<Breadcrumb category={this.state.category} vacationCategory={this.state.vacationCategory}/>
			<div id="content">
				<section className="wrap">
					<header className="title">
						<h2 className="float-l">各國遊學行程</h2>
						<ul className="category">
						{
							this.state.vacationCategory.map(function(itemData,i) {
								if(category==itemData.val){
									var subOutHtml =<li key={i}><a href={gb_approot + 'StudyTravel?category='+itemData.val} className="current">{itemData.Lname}</a></li>;
								}else{
									var subOutHtml =<li key={i}><a href={gb_approot + 'StudyTravel?category='+itemData.val}>{itemData.Lname}</a></li>;
								}
								return subOutHtml;
							}.bind(this))
						}
						</ul>
						<div className="select">
							<label for="">選擇國家</label>
							<select onChange={this.countryChange}>
								<option value="">全部國家</option>
								{
									this.state.countryCategory.map(function(itemData,i) {
											var subOutHtml =<option key={i} value={itemData.val}>{itemData.Lname}</option>;
										return subOutHtml;
									}.bind(this))
								}
							</select>
						</div>
					</header>

					<section className="article-list">
						<header className="hidden"><h3>Hot行程</h3></header>
						{
							this.state.gridData.rows.map(function(itemData,i) {
								var subOutHtml =
								<article className="pic-article" key={i}>
									<a href={this.props.contextUrl + itemData.study_abroad_id} className="thumb"><img src={itemData.imgsrc}/></a>
									<header><h4>{itemData.planning_name}</h4></header>
								    <em>{moment(itemData.start_date).format('YYYY-MM-DD')}~{moment(itemData.end_date).format('YYYY-MM-DD')}</em>
								  	<p dangerouslySetInnerHTML={this.createMarkupHtml(itemData.intro)}></p>
									<a href={this.props.contextUrl + itemData.study_abroad_id} className="btn">查看更多</a>
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
	getDefaultProps:function(){
		return{
			category:null,
			vacationCategory:[]
		};
	},
	categroyFilter:function(){
		var plan="";
		this.props.vacationCategory.forEach(function(object, i){
			if(this.props.category==object.val){
				plan=object.Lname;
			}
		}.bind(this))
		return plan;
	},
	render:function(){
		var outHtml = (
			<ul className="breadcrumb">
				<li><a href="Index">首頁</a></li>
				<li><a href={gb_approot + 'StudyTravel'} >各國遊學行程</a></li>
				<li>{this.categroyFilter()}</li>
			</ul>
		);

		return outHtml;
	}
});

var comp = React.render(<PageContent dataUrl={gb_approot + 'api/GetAction/GetStudyAbroadWWW'} contextUrl={gb_approot + 'StudyTravel/p1_content?id='} />,document.getElementById('PageContent'));