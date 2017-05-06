import React from 'react';
import ReactNative from 'react-native';
import {
  AppRegistry,
  StyleSheet,
  Text,
  View,
  Image, // we want to use an image
  PanResponder, // we want to bring in the PanResponder system
  Animated // we wil be using animated value
} from 'react-native';
import {Component} from 'react';

//import App from './Main';
//import Game from './Pages/Games';


class stroke_survivor_app extends Component {

  constructor(props) {
    super(props);

    this.state = {
      pan: new Animated.ValueXY(),
      scale: new Animated.Value(1)
    };
  }

  componentWillMount() {
    this._panResponder = PanResponder.create({
      onMoveShouldSetResponderCapture: () => true,
      onMoveShouldSetPanResponderCapture: () => true,

      onPanResponderGrant: (e, gestureState) => {
        // Set the initial value to the current state
        this.state.pan.setOffset({x: this.state.pan.x._value, y: this.state.pan.y._value});
        this.state.pan.setValue({x: 0, y: 0});
        Animated.spring(
          this.state.scale,
          { toValue: 1.1, friction: 3 }
        ).start();
      },

      // When we drag/pan the object, set the delate to the states pan position
      onPanResponderMove: Animated.event([
        null, {dx: this.state.pan.x, dy: this.state.pan.y},
      ]),

      onPanResponderRelease: (e, {vx, vy}) => {
        // Flatten the offset to avoid erratic behavior
        this.state.pan.flattenOffset();
        Animated.spring(
          this.state.scale,
          { toValue: 1, friction: 3 }
        ).start();
      }
    });
  }

  render() {
    // Destructure the value of pan from the state
    let { pan, scale } = this.state;

    // Calculate the x and y transform from the pan value
    let [translateX, translateY] = [pan.x, pan.y];

    let rotate = '0deg';

    // Calculate the transform property and set it as a value for our style which we add below to the Animated.View component
    let imageStyle = {transform: [{translateX}, {translateY}, {rotate}, {scale}]};

    return (
        <View style={styles.container}>
        <Animated.View style={imageStyle} {...this._panResponder.panHandlers}>
          <Image
            style={styles.ball}
            source={require('./Assets/red-ball.png')} />
        </Animated.View>
      </View>
      //<View>
        //<Animated.View
        //    style={imageStyle} {...this._panResponder.panHandlers}>
          //<Image
            //style={styles.container}
            //source={require('./Assets/red-ball.png')}
          ///>
        //</Animated.View>
      //</View>
    );
  }

}


const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#F5FCFF',
  },
  welcome: {
    fontSize: 20,
    textAlign: 'center',
    margin: 10,
  },
  ball: {
    width: 100,
    height: 100,
    borderRadius: 100/2,
    backgroundColor: 'red'
  },
  instructions: {
    textAlign: 'center',
    color: '#333333',
    marginBottom: 5,
  },
});

AppRegistry.registerComponent('app', () => stroke_survivor_app);
