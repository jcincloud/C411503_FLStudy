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
		this.bannerEffect();

		return;
	},
	bannerEffect:function(){
		 $('#banner').flexslider({
            pauseOnAction: false
        });
	},
	render:function(){
		var outHtml = (
			 <ul className="slides">
			 {/*}
			 <li>
            		<figure>
                		<img src={gb_approot+'Content/images/StudyA/banner1.jpg'} alt="" />
                		<figcaption><p>test</p></figcaption>
            		</figure>
        	</li>
        				 <li>
            		<figure>
                		<img src={gb_approot+'Content/images/StudyA/banner1.jpg'} alt="" />
                		<figcaption><p>test</p></figcaption>
            		</figure>
        	</li>*/}
			   	{
                    this.state.contentData.benner_imgsrc.map(function(itemData,i) {
                    	var subOutHtml = 
                    		<li key={i}>
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

//banner元件嵌入 div id:banner
React.render(<StudyBanner />,document.getElementById('banner'));