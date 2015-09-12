//[1]
//主元件 
var PageContent = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return { 
			contentData:{photoB_imgsrc:[],photoS_imgsrc:[]},
			vacationCategory:[],
			initPathName:gb_approot+'StudyA/axContentInit'
		};  
	},		
	componentDidMount:function(){
		this.getAjaxInitData();//載入initdata
		jqGet(gb_approot + 'api/GetAction/GetStudyAbroadContentWWW',{id:gb_id})
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({contentData:data});
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
		return;
	},
	getAjaxInitData:function(){
		jqGet(this.state.initPathName)
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({vacationCategory:data.options_vacation_category});
			//載入下拉是選單內容
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	createMarkupHtml:function(content){
		if(content!=undefined){
			return {__html: content };
		}else{
			return {__html: "" };			
		}
	},
	render:function(){

		var contentData=this.state.contentData;

		var outHtml = (
		<div>
			<Breadcrumb titleName={contentData.planning_name} category={contentData.vacation_category} vacationCategory={this.state.vacationCategory}/>
			<div id="content">

				<header className="title">
					<a href={gb_approot + 'StudyA'} className="btn goBack">回列表</a>
					<h2>{contentData.planning_name}</h2>
				</header>

				<section className="intro">
					<div className="wrap">
						<header>
							<h3>{contentData.intro_titile + '行程簡介'}<em>Introduction</em></h3>
						</header>
						<div className="article-content" dangerouslySetInnerHTML={this.createMarkupHtml(contentData.intro_content)}></div>
					</div>
				</section>

				<div className="gallery">
					<div className="wrap">
						<ul className="tab-nav">
							<li className="photo" rel="#tab1">
                    			<em>照片集錦<i>Photos</i></em>
                			</li>
                			<li className="video" rel="#tab2">
                    			<em>影片分享<i>Videos</i></em>
                			</li>
						</ul>
						<div className="tab-content">
							<section className="photo" id="tab1">
                    			<header className="hidden"><h3>照片集錦<em>Photos</em></h3></header>
                    				<ul className="gallery-slider">
                    				{
                    					this.state.contentData.photoB_imgsrc.map(function(itemData,i) {
                    						var subOutHtml = 
                    						<li key={i}>
                    						<img src={itemData} alt="" />
                    						</li>;
                    						return subOutHtml;
                    					}.bind(this))
                    				}
                    				</ul>
                    			<div className="gallery-slider-nav als-container">
                        			<span className="als-prev"><i className="fa-angle-up"></i></span>
                        			<div className="als-viewport">
                            			<ul className="als-wrapper">
                            		{
                    					this.state.contentData.photoS_imgsrc.map(function(itemData,i) {
                    						var subOutHtml = 
                    						<li className="als-item" key={i}>
                    						<img src={itemData} alt="" />
                    						</li>;
                    						return subOutHtml;
                    					}.bind(this))
                    				}
                            			</ul>
                        			</div>
                        			<span className="als-next"><i className="fa-angle-down"></i></span>
                    			</div>
                			</section>
							<section className="video" id="tab2">
                    			<header className="hidden"><h3>影片分享<em>Videos</em></h3></header>
                    			<div className="embed" dangerouslySetInnerHTML={this.createMarkupHtml(contentData.youtube_iframe)}></div>
                			</section>
						</div>
					</div>
				</div>

				<section className="course">
        			<div className="wrap">
            			<header>
                			<h3>{contentData.intro_titile + '課程與住宿'}<em>Courses &amp; Accommodation</em></h3>
            			</header>
            			<article className="pic-article">
                			<header><h4>課程</h4></header>
                			<img src={gb_approot+'Content/images/StudyA/pic2.jpg'} alt="" className="thumb" />
                			<p dangerouslySetInnerHTML={this.createMarkupHtml(contentData.curriculum)}></p>
            			</article>
            			<article className="pic-article">
                			<header><h4>住宿</h4></header>
                			<img src={gb_approot+'Content/images/StudyA/pic3.jpg'} alt="" className="thumb" />
                			<p dangerouslySetInnerHTML={this.createMarkupHtml(contentData.lodging)}></p>
           				 </article>
            			<article className="pic-article">
                			<header><h4>活動</h4></header>
               			 	<img src={gb_approot+'Content/images/StudyA/pic4.jpg'} alt="" className="thumb" />
                			<p dangerouslySetInnerHTML={this.createMarkupHtml(contentData.activity)}></p>
           				</article>
        			</div>
    			</section>

    			<ContactUs />
			</div>
		</div>
			);

		return outHtml;
	}
});

//[2]
//麵包屑元件
var Breadcrumb = React.createClass({
	getDefaultProps:function(){
		return{
			titleName:null,
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
				<li><a href={gb_approot + 'StudyA'}>各國遊學行程</a></li>
				<li><a href={gb_approot +'StudyA?category='+this.props.category}>{this.categroyFilter()}</a></li>
				<li>{this.props.titleName}</li>
			</ul>
		);

		return outHtml;
	}
});

//[3]
//聯絡我們元件
var ContactUs = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return { 
			validateUrl: this.getValidateUrl()
		};  
	},
	getValue:function(){
		return this.state.value;
	},
	onChange:function(event){//大小寫轉換
		this.setState({value:event.target.value.toUpperCase()});
	},
	getValidateUrl:function(){//抓取驗證碼圖片連結
		return gb_approot + '_Code/Ashx/ValidateCode.ashx?vn=ContactUs&t=' + uniqid();
	},
	reLoadValidateUrl:function(){//重取驗證碼
		this.setState({validateUrl: this.getValidateUrl(),value:null});
	},
	render:function(){
		var outHtml = (
		<div className="orientation">
			<div className="wrap">
			    <section className="join">
                	<header>
                    	<h3>參加免費遊學說明會<em>Orientation</em></h3>
                	</header>
                	<div className="article-content">
                    	<p>如何知道自己是否適合海外遊學活動？如何選擇適合的遊學課程或地點？</p>
                    	<p>住宿海外接待家庭與宿舍的差別在哪？國外的教學課程與旅遊活動的內容？<br />遊學活動究竟可以有什麼樣的體驗？</p>
                		<p>富學遊學說明會，提供您在規劃海外遊學時應該瞭解的重要資訊；<br />一個小時的說明會，協助您正確的評估海外遊學到底是什麼？是不是符合自己的計劃和需求！</p>
                    	<p><strong>說明會座位有限~提早預約可確保座位</strong></p>
                    	<p>現在立刻填寫資料即可免費預約參加說明會 <i class="fa-caret-right"></i></p>
                	</div>
            	</section>
            	<aside className="apply">
                	<header><h3>免費預約參加</h3></header>
                    <form id="ContactUs">
                    	<p><input type="text" placeholder="姓名" /></p>
                    	<p><input type="text" placeholder="E-mail" /></p>
                    	<p><input type="text" placeholder="電話 / 手機" /></p>
                    	<p className="code">
                        	<img src={this.state.validateUrl} alt="驗證碼" />
                        	<input type="text" placeholder="請輸入圖中的文字" onChange={this.onChange} value={this.state.value}/>
                    	</p>
                    	<p><button type="button">送出資料</button></p>
                	</form>
            	</aside>
			</div>
		</div>
		);

		return outHtml;
	}
});

//[4]
//banner元件
var StudyBanner = React.createClass({
		getInitialState: function() {  
		return { 
			contentData:{benner_imgsrc:[]}
		};  
	},	
	componentDidMount:function(){
		jqGet(gb_approot + 'api/GetAction/GetStudyAbroadbBannerWWW',{id:gb_id})
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({contentData:data});
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});

		return;
	},
	render:function(){
		var outHtml = (
			 <ul className="slides">
			 {/*<li>
            		<figure>
                		<img src={gb_approot+'Content/images/StudyA/banner1.jpg'} alt="" />
                		<figcaption><p>test</p></figcaption>
            		</figure>
        	</li>*/}
			   	{
                    this.state.contentData.benner_imgsrc.map(function(itemData,i) {
                    	var subOutHtml = 
                    		<li>
           						<figure>
                				<img src={itemData} alt="" />
                				<figcaption><p>{this.state.contentData.planning_name}</p></figcaption>
            					</figure>
        					</li>;
                    	return subOutHtml;
                    }.bind(this))
                }
    		</ul>
		);

		return outHtml;
	}
});




//元件嵌入 div id:PageContent
var comp = React.render(<PageContent contextUrl={gb_approot + 'api/GetAction/GetStudyAbroadContentWWW?id='} />,document.getElementById('PageContent'));

