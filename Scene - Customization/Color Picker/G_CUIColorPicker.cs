/*
https://github.com/SnapshotGames/cui_color_picker

MIT License

Copyright (c) 2016 Snapshot Games Inc.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using UnityEngine;
using UnityEngine.UI;
#if GRAPHY_NEW_INPUT
using UnityEngine.InputSystem;
#endif

namespace Tayx.Graphy.CustomizationScene
{
    public class G_CUIColorPicker : MonoBehaviour
    {
        public Color Color { get { return _color; } set { Setup( value ); } }
        
        public void SetOnValueChangeCallback( System.Action<Color> onValueChange )
        {
            _onValueChange = onValueChange;
        }
        
        [SerializeField] private Slider alphaSlider             = null;
                
        [SerializeField] private Image alphaSliderBGImage       = null;

        private Color _color                                    = new Color32(255, 0, 0, 128);
        private System.Action<Color> _onValueChange             = null;
        private System.Action _update                           = null;
    
        private static void RGBToHSV( Color color, out float h, out float s, out float v )
        {
            var cmin = Mathf.Min( color.r, color.g, color.b );
            var cmax = Mathf.Max( color.r, color.g, color.b );
            var d = cmax - cmin;
            if ( d == 0 ) {
                h = 0;
            } else if ( cmax == color.r ) {
                h = Mathf.Repeat( ( color.g - color.b ) / d, 6 );
            } else if ( cmax == color.g ) {
                h = ( color.b - color.r ) / d + 2;
            } else {
                h = ( color.r - color.g ) / d + 4;
            }
            s = cmax == 0 ? 0 : d / cmax;
            v = cmax;
        }
    
        private static bool GetLocalMouse( GameObject go, out Vector2 result ) 
        {
            var rt = ( RectTransform )go.transform;
#if GRAPHY_NEW_INPUT
            var mp = rt.InverseTransformPoint( Mouse.current.position.ReadValue() );
#else
            var mp = rt.InverseTransformPoint( Input.mousePosition );
#endif
            result.x = Mathf.Clamp( mp.x, rt.rect.min.x, rt.rect.max.x );
            result.y = Mathf.Clamp( mp.y, rt.rect.min.y, rt.rect.max.y );
            return rt.rect.Contains( mp );
        }
    
        private static Vector2 GetWidgetSize( GameObject go ) 
        {
            var rt = ( RectTransform )go.transform;
            return rt.rect.size;
        }
    
        private GameObject GO( string name )
        {
            return transform.Find( name ).gameObject;
        }
    
        private void Setup( Color inputColor )
        {
            alphaSlider.value = inputColor.a;
            alphaSliderBGImage.color = inputColor;
            
            var satvalGO = GO( "SaturationValue" );
            var satvalKnob = GO( "SaturationValue/Knob" );
            var hueGO = GO( "Hue" );
            var hueKnob = GO( "Hue/Knob" );
            var result = GO( "Result" );
            var hueColors = new Color [] 
            {
                Color.red,
                Color.yellow,
                Color.green,
                Color.cyan,
                Color.blue,
                Color.magenta,
            };
            
            var satvalColors = new Color [] 
            {
                new Color( 0, 0, 0 ),
                new Color( 0, 0, 0 ),
                new Color( 1, 1, 1 ),
                hueColors[0],
            };
            
            var hueTex = new Texture2D( 1, 7 );
            
            for ( int i = 0; i < 7; i++ ) {
                hueTex.SetPixel( 0, i, hueColors[i % 6] );
            }
            
            hueTex.Apply();
            hueGO.GetComponent<Image>().sprite = Sprite.Create( hueTex, new Rect( 0, 0.5f, 1, 6 ), new Vector2( 0.5f, 0.5f ) );
            var hueSz = GetWidgetSize( hueGO );
            var satvalTex = new Texture2D(2,2);
            satvalGO.GetComponent<Image>().sprite = Sprite.Create( satvalTex, new Rect( 0.5f, 0.5f, 1, 1 ), new Vector2( 0.5f, 0.5f ) );

            System.Action resetSatValTexture = () => {
                for ( int j = 0; j < 2; j++ ) {
                    for ( int i = 0; i < 2; i++ ) {
                        satvalTex.SetPixel( i, j, satvalColors[i + j * 2] );
                    }
                }
                satvalTex.Apply();
            };
            
            var satvalSz = GetWidgetSize( satvalGO );
            float Hue, Saturation, Value;
            RGBToHSV( inputColor, out Hue, out Saturation, out Value );

            System.Action applyHue = () => 
            {
                var i0 = Mathf.Clamp( ( int )Hue, 0, 5 );
                var i1 = ( i0 + 1 ) % 6;
                var resultColor = Color.Lerp( hueColors[i0], hueColors[i1], Hue - i0 );
                satvalColors[3] = resultColor;
                resetSatValTexture();
            };

            System.Action applySaturationValue = () => 
            {
                var sv = new Vector2( Saturation, Value );
                var isv = new Vector2( 1 - sv.x, 1 - sv.y );
                var c0 = isv.x * isv.y * satvalColors[0];
                var c1 = sv.x * isv.y * satvalColors[1];
                var c2 = isv.x * sv.y * satvalColors[2];
                var c3 = sv.x * sv.y * satvalColors[3];
                var resultColor = c0 + c1 + c2 + c3;
                var resImg = result.GetComponent<Image>();
                resImg.color = resultColor;
                if ( _color != resultColor )
                {
                    resultColor = new Color(resultColor.r, resultColor.g, resultColor.b, alphaSlider.value);

                    _onValueChange?.Invoke( resultColor );

                    _color = resultColor;
                    
                    alphaSliderBGImage.color = _color;
                }
            };
            applyHue();
            applySaturationValue();
            satvalKnob.transform.localPosition = new Vector2( Saturation * satvalSz.x, Value * satvalSz.y );
            hueKnob.transform.localPosition = new Vector2( hueKnob.transform.localPosition.x, Hue / 6 * satvalSz.y );
            System.Action dragH = null;
            System.Action dragSV = null;
            System.Action idle = () => {
#if GRAPHY_NEW_INPUT
                if ( Mouse.current.leftButton.wasPressedThisFrame ) {
#else
                if ( Input.GetMouseButtonDown( 0 ) )
                {
#endif
                    Vector2 mp;
                    if ( GetLocalMouse( hueGO, out mp ) ) {
                        _update = dragH;
                    } else if ( GetLocalMouse( satvalGO, out mp ) ) {
                        _update = dragSV;
                    }
                }
            };
            dragH = () => {
                Vector2 mp;
                GetLocalMouse( hueGO, out mp );
                Hue = mp.y / hueSz.y * 6;
                applyHue();
                applySaturationValue();
                hueKnob.transform.localPosition = new Vector2( hueKnob.transform.localPosition.x, mp.y );
#if GRAPHY_NEW_INPUT
                if ( Mouse.current.leftButton.wasReleasedThisFrame ) {
#else
                if ( Input.GetMouseButtonUp( 0 ) )
                {
#endif
                    _update = idle;
                }
            };
            dragSV = () => {
                Vector2 mp;
                GetLocalMouse( satvalGO, out mp );
                Saturation = mp.x / satvalSz.x;
                Value = mp.y / satvalSz.y;
                applySaturationValue();
                satvalKnob.transform.localPosition = mp;
#if GRAPHY_NEW_INPUT
                if ( Mouse.current.leftButton.wasReleasedThisFrame ) {
#else
                if ( Input.GetMouseButtonUp( 0 ) )
                {
#endif
                    _update = idle;
                }
            };
            _update = idle;
        }
    
        public void SetRandomColor()
        {
            var rng = new System.Random();
            var r = ( rng.Next() % 1000 ) / 1000.0f;
            var g = ( rng.Next() % 1000 ) / 1000.0f;
            var b = ( rng.Next() % 1000 ) / 1000.0f;
            Color = new Color( r, g, b );
        }
    
        void Awake()
        {
            Color = new Color32(255, 0, 0, 128);
        }

        private void Start()
        {           
            alphaSlider.onValueChanged.AddListener(value =>
            {
                _color = new Color(_color.r, _color.g, _color.b, value);

                alphaSliderBGImage.color = _color;

                _onValueChange?.Invoke( _color );
            } );
        }

        void Update()
        {
            _update?.Invoke();
        }
    } 
}


