//[1]
//聯絡我們元件
var ContactUs = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return { 
			validateUrl: this.getValidateUrl(),
			fieldData:{}
		};  
	},
	getDefaultProps:function(){
		return{	
			fdName:'fieldData',
			mailPathName:gb_approot+'StudyTravel/sendMail'

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
	changeFDValue:function(name,e){
		this.setInputValue(this.props.fdName,name,e);
	},
	setInputValue:function(collentName,name,e){

		var obj = this.state[collentName];
		if(e.target.value=='true'){
			obj[name] = true;
		}else if(e.target.value=='false'){
			obj[name] = false;
		}else{
			obj[name] = e.target.value;
		}
		this.setState({fieldData:obj});
	},
	sendMail:function(){
		var fieldData = this.state.fieldData;
		if(fieldData.send_name==null){
			alert("姓名栏位空白!");
		}else if(fieldData.email==null){
			alert("email栏位空白!");					
		}else if(fieldData.vaild==null){
			alert("验证码栏位空白!");		
		}else{
			jqPost(this.props.mailPathName,this.state.fieldData)
			.done(function(data, textStatus, jqXHRdata) {
				if(data.result){
					alert("感谢您的填写，我们收到资料后将会尽快与您联系！");					
				}else{
					alert(data.message);
					this.reLoadValidateUrl();
				}
			}.bind(this))
			.fail(function( jqXHR, textStatus, errorThrown ) {
				showAjaxError(errorThrown);
			});
		}		
	},
	render:function(){
		var fieldData = this.state.fieldData;
		var outHtml = (
            <div>
                <form>
                    <p><input type="text" placeholder="姓名" value={fieldData.send_name} onChange={this.changeFDValue.bind(this,'send_name')} maxLength="64" required /></p>
                    <p><input type="email" placeholder="E-mail" value={fieldData.email} onChange={this.changeFDValue.bind(this,'email')} maxLength="255" required /></p>
                   	<p><input type="text" placeholder="电话 / 手机" value={fieldData.tel} onChange={this.changeFDValue.bind(this,'tel')} maxLength="25" /></p>	
                   	<p className="code">
                       	<img src={this.state.validateUrl} alt="验证码"/>
                       	<input type="text" placeholder="请输入图中的文字" value={fieldData.vaild} onChange={this.changeFDValue.bind(this,'vaild')} maxLength="5" required/>
                   	</p>
                   	<p><button type="button" onClick={this.sendMail}>送出资料</button></p>
                </form>
            </div>
		);

		return outHtml;
	}
});


//聯絡我們元件嵌入 div id:contactUs
React.render(<ContactUs />,document.getElementById('ContactUsContent'));