using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace qtools.qhierarchy.pdata
{
	public enum QTexture
	{
		QCheckBoxChecked = 0,
		QCheckBoxUnchecked = 1,
		QColorButton = 2,
		QColorPalette = 3,
		QComponentUnknownIcon = 4,
		QDragButton = 5,
		QErrorIcon = 6,
		QLockButton = 7,
		QMonoBehaviourIcon = 8,
		QPrefabIcon = 9,
		QRendererButton = 10,
		QRestoreButton = 11,
		QTreeMapCurrent = 12,
		QTreeMapLast = 13,
		QTreeMapLevel = 14,
		QTreeMapLevel4 = 15,
		QTreeMapObject = 16,
		QTrimIcon = 17,
		QVisibilityButton = 18,
		QVisibilityOffButton = 19,
	};

	public enum QColor
	{
		BackgroundDark,
		Background,
		Gray,
		GrayLight,
		GrayDark
	}

	public class QResources
	{
		// SINGLETON
		private static QResources instance;
		public static QResources getInstance()
		{
			if (instance == null) instance = new QResources();
			return instance;
		}

		// PRIVATE
		private Dictionary<QTexture, Texture2D> textures;
		private Dictionary<QTexture, string> resourcesCommon = new Dictionary<QTexture, string>()
		{
			{ QTexture.QColorButton, "iVBORw0KGgoAAAANSUhEUgAAAAgAAAAQCAYAAAArij59AAAAWUlEQVQoFWP8//8/Az7AhE8SJDcYFLBAHakLpIOB2AGIDwDxWiC+DMQMMAUgSR+QABDAaLACmC8cwFIIAs6HKTiAkAOz4HyYFSA7QcABiA8AMYzPwDgUghoAHO8PN+sTbZ4AAAAASUVORK5CYII=" },
			{ QTexture.QColorPalette, "iVBORw0KGgoAAAANSUhEUgAAAJYAAAA8CAYAAACEhkNqAAADBklEQVR4Ae2dT2sTQRyGZ5uALZaqhxJvevTixasHJR9Mqgc/gDfvkoP4IeqfowcVFPSSBDwYsCBpTY20jTuxC+a3dvKblzk+e1lm5n2n5eFhdguhqRb1FcTrd7cSm3Wtr1d/PdySy092b8ndp7NncnfrzTW5G/b16uN3evdueCGXN+QmRQgkCCBWAg5LOgHE0tnRTBBoiXX6aC+cvdxfqcRxnOeCgJdAS6zq3v2wqEVq5Ir3OI7zXBDwEuja4EYt0Fk9uZRrNAqL8WgpVZzngoCXQOvEisUoUXXj5l+p6jtSeXGSawj8V6zl4y+eVOdyNY/FpsQdAusItB6F/75TLR+L5+9Y8fHIybUOJ+sNgdaJ1byoNxLFe/NC35S4Q2AdgdaJ1Xmw1+osJePlvcWFiYsJtE6si6OsQMBPALH8rEhmEECsDFhE/QS6w+HQnzbJ+ellM5MxnPzMCK9GO5/i36ja9bE31Yp16/jwi9w9GW/L3XCgVz8f6t2rYSyXObFkdBRTBBArRYc1mQBiyegopgggVooOazIBxJLRUUwRQKwUHdZkAoglo6OYIoBYKTqsyQQQS0ZHMUUAsVJ0WJMJIJaMjmKKAGKl6LAmE0AsGR3FFAHEStFhTSbQHQwGcnm7uiR3w7eZ3N15dSJ33+/+kLuzo9dytzNsfQrcv9dXf9Qm3x7ZGf94Gj74wybJiWWAMCxDALHKcGQXQwCxDBCGZQggVhmO7GIIIJYBwrAMAcQqw5FdDAHEMkAYliGAWGU4soshgFgGCMMyBBCrDEd2MQQQywBhWIYAYpXhyC6GAGIZIAzLEECsMhzZxRCoer2e/CVN/cncbJcx3NQ/vrJzR/9yqOdXNjN+ydXodN5fncgYVQf6f8gJk4wfZKK3v5uJjOH1oH9ehxMrAzRRPwHE8rMimUEAsTJgEfUTQCw/K5IZBBArAxZRPwHE8rMimUEAsTJgEfUTQCw/K5IZBBArAxZRPwHE8rMimUEAsTJgEfUTQCw/K5IZBBArAxZRPwHE8rMimUHgD7Cif5j2Lp5yAAAAAElFTkSuQmCC" },
			{ QTexture.QComponentUnknownIcon, "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAABGElEQVQ4EWNgoBAwQvVLAWk1Isy6BVTzDFkdC5Sj9ujZy/3IEsjsD5+/MixdtJChs7XRESoON4QJWSEuNgc7K0NwRBRDbVMbyBKQS0EuBgPiDGBjYxDg4WLwDQxmqG/tghmC24CP334yZM3fziCfN4nBp3sFw+XHrxg42CGGOLt7Qu3G44Jlx64wHLn5iGFpdiCDnAg/Q9a87WDVIEPEhPhRDIAFIoqgjbosg66sGAOIBtkOMgwGuDjYYUwwjdUAkGYQePT2I8P0PWcYMl1MwHxsBN5ArFyxD+ySTBdjbHrBYngNAKnAZztIHq8BNupyIDV4AV4DOjYdBQciPhOwBiJMw8NJeTAmThqvC3DqQpIgNTfCtMJzJQDmf0F9Rh99OwAAAABJRU5ErkJggg==" },
			{ QTexture.QErrorIcon, "iVBORw0KGgoAAAANSUhEUgAAAAcAAAAQCAYAAADagWXwAAAANklEQVQYGWP8/fs3Ay7AhEsCJE605H+gYhCGA6J1wnXAGAOhkwVmOZBmRGKDmUQ7iLRAIN9OAA9DBxP0TyMiAAAAAElFTkSuQmCC" },
			{ QTexture.QLockButton, "iVBORw0KGgoAAAANSUhEUgAAAA0AAAAQCAYAAADNo/U5AAAAtklEQVQoFb2QsQ0CMQxFY6Cgp+EmYA4WgDVYhtuFLViBCagoKY/wXxRHXJDCQcGXvuxvf599sRhj+BaLaqCT3osr8SaexKs4Bpsyd4p38RVo6u5J0UWnBoazuBGXOaKp03dv8OSgImDAa0Q0oF/qs3zsOsfL+Pjg2vup7UOV94PU2l4cxBbo40snmpJB352y8SHfnBswvw2Y2ZZmheIrSWVoyv8N/fwQR/0AL9gCfXwJbPJ8cnwCewTKXVfaQ3EAAAAASUVORK5CYII=" },
			{ QTexture.QMonoBehaviourIcon, "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAMUlEQVQ4EWP8//8/AyWABagZZEIhuYYwkasRpm/UAAaG0TAYDQNQfgClgz+wjEEODQAZqgWLOZX9TgAAAABJRU5ErkJggg==" },
			{ QTexture.QPrefabIcon, "iVBORw0KGgoAAAANSUhEUgAAAAkAAAAQCAYAAADESFVDAAAAd0lEQVQoFY2RUQqAMAxDV/GeHkU8ijfzHv3QphIJm7AWtpbtkWTM3L3NapkBuC9Ba4D3j5rpGSDU8bbcd5lzLNmVINpBdhMb5sxsvdIZ4BVLMzYqMayqfcKAUjI6LKA0VG83ADgoQSYfzBepWkZhcFwwm0I5l+weLU0O7oJcg0oAAAAASUVORK5CYII=" },
			{ QTexture.QRendererButton, "iVBORw0KGgoAAAANSUhEUgAAAAwAAAAQCAYAAAAiYZ4HAAAAdElEQVQoFWP8/v07AwsLCwOxgIlYhTB1JGsAuUUFiGVgJhCgDwCdz3KbgCKYtCOIAXYSKyurIxAzggRwsUFyIIDsBwewCITAxWZg/P37938khfiYICcdGHUSviD68+ePKlD+DihpgCMMn2JkOeSIQxbHyQYAcE0cpIy04qQAAAAASUVORK5CYII=" },
			{ QTexture.QRestoreButton, "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAA3klEQVQ4Ec1RMQ7CMAxMUtj4CSPiATyADzB2QWx8gAlF4geIpTO/YEZIbAz8gxWFuyip3KRR2cDS1fad7dSJttaqkjnnvKS1LpUoU1S+FP53wBobXIFXAGNymfWtwMIdcDfGLAjGgcuGjCC0xlvHjR9AnIFNKyh1C3ENfxJ89gpbihj0DN7X8hmBBvyUh0jIFd6onkEc0wPMB02uUOGU2LRCJ3M/gCfCauQPT4iP/APSVdCiZzoHjsASaICOyT+Igm+Oe4K8hJP3iDsXyIa+AeSlTWSSxukKqT6Y/37AB6sOP8hny1/VAAAAAElFTkSuQmCC" },
			{ QTexture.QTreeMapCurrent, "iVBORw0KGgoAAAANSUhEUgAAAA4AAAAQCAYAAAAmlE46AAAAQ0lEQVQoFWNgwA+E/wMBUIkwujImdAFi+aMa8YQUCx45dCmUKCFKIzAq36CbQpRGRkZGEbI0QjW9RdY8mgCQQwONDQApiglJmB+fmgAAAABJRU5ErkJggg==" },
			{ QTexture.QTreeMapLast, "iVBORw0KGgoAAAANSUhEUgAAAA4AAAAQCAYAAAAmlE46AAAAPUlEQVQoFWNgwA+E/wMBUIkwujImdAFi+aMa8YQUCx45dCmUKCFKIzAq36CbwogugMbnBvI50MRGuTQLAQD/rQhHffk54gAAAABJRU5ErkJggg==" },
			{ QTexture.QTreeMapLevel, "iVBORw0KGgoAAAANSUhEUgAAAA4AAAAQCAYAAAAmlE46AAAAJElEQVQoFWNgwA8k/wMBUIkkujImdAFi+aMa8YTUaOCM8MABAI00BE1+cZ4yAAAAAElFTkSuQmCC" },
			{ QTexture.QTreeMapLevel4, "iVBORw0KGgoAAAANSUhEUgAAADgAAAAQCAYAAABDebxFAAAATklEQVRIDe2SMQoAMAgDpV/w/29t3QWzpnKOGiHmjJgrb1VJcpa1qc3eadaWNTjwd6AQhKB5AryoOSBpD4IyInMBBM0BSXsQlBGZC9YTfL7XEKcUdfHdAAAAAElFTkSuQmCC" },
			{ QTexture.QTreeMapObject, "iVBORw0KGgoAAAANSUhEUgAAAA4AAAAQCAYAAAAmlE46AAABnUlEQVQoFdVSTW/aQBR83vX6K9iOkAqmQVUQyoUrUg7cg/qLkTjliDggVfTKZ9rKiT9Q2hjLLsTprMWBWL311Lcar/12Z+fN8xL9N6FUKlXxbQHOCTZmBqTAM/Dr9H6QG8+jPhgM7hqNxnW73b7wPK8mhGBxGKfbb9uX1WoVxnH8FfO8SrwaDoe3rusatu2Srovy0I+e53Zvum6v1/uw2Ww+gfhUJV4WRWEsl0uyrBp1Oh0yTUE/fJ+iICKs6a1Wq8BpTpXIuMLfsEEJgoCiKICqTsfjkegN5i8syrIMH8Sk8feBjFAFFA1KkoTDkyaYIKEByHPOy4ZWFQtd1RSmgA0FNIZIFoZgGCpXybAMKXaoKv5M0v1vprKSxFBQgcEEQwUWCTRrNpvtQNxxedpZvMa72K3X67ZpmTq8gcuY4zivh+Mhm0wm36fT6TjP822VmIdh+LBerxN4cZvNpq1pGvd9/3E0Gn1ZLBbjNE2XEMqrN0eKy5wJf91+v/8ZHbbw3+6jKJojL29O6fpvRKyVoeFZA2Qf9kAGoGX/GH8AjXiXWwSceRAAAAAASUVORK5CYII=" },
			{ QTexture.QTrimIcon, "iVBORw0KGgoAAAANSUhEUgAAAAcAAAAQCAYAAADagWXwAAAAOUlEQVQYGWP8//8/Ay7AhEsCJD5YJOHOR3cQXALdtSgS6JKMIAFkgG4sigJ0SZBGuAJsknCTaSQJAGHZBh0Iaq7CAAAAAElFTkSuQmCC" },
			{ QTexture.QVisibilityButton, "iVBORw0KGgoAAAANSUhEUgAAABIAAAAQCAYAAAAbBi9cAAABBklEQVQ4EdWTuw4BURCGXRqXXkmvoBXeg5dRU1MpNlHwBBoKUai1GhQKQkmDKKzv3+zIuhQk2/iTz4yZs+PMzIq6rhsJQ7EwiqjGfxQqctM2zOEEF1iCAxX4LA3bJ45twhWkEwxhAAeQbuBAGuw5z9qXKIk+mGY4GVBeuRSMwDTGUcyej5hTtxNY3SQLFVjDHmqQgA2YOjjqwquhjzwEf23iJ1dYk1rT2a4FsFMogVdI6w/njfQrvraWI26t7fCrkIQtmN5a09U00J6dwAaHrfzXw9ZhDa4Btv4zvman9R9BsvU/bYz4Y2vewBSAArRgDtrgBRbgQBmCZx++Wvr8pv4YDe1PeweSfPysEmODwwAAAABJRU5ErkJggg==" },
			{ QTexture.QVisibilityOffButton, "iVBORw0KGgoAAAANSUhEUgAAABIAAAAQCAYAAAAbBi9cAAABdUlEQVQ4Ea3TvS8EQRjH8dvLHZGoLvQqQaKReKlEp6RA7Q9QafwDGp1EIgqRy3mJhASJSiMaCi4Roj2rOAonkmtEgvX9ze6zVrKh2Sf5zPPM7Mxkd+bOC4Igl0Xks9hEe2S+0TibVlLersCY/Bv2Rs/MnMR2tGKAfIkafJxjET1IDx12pI+sKCOPLdzhAu9QKC+hCFvncrKzzEOLfQp71kJdwpk9JB+hFTYnZ8VCYtIrdR2H0cQ58iza8QSLFQoPbg+dkb57LPHhV9RDmMAe1tCNN5zAop9C81zoRoqRcCRs66R7TEG/2BkovDC5Vi+htS7UucV12HXtIG0J82hiGmUodLsWjxRV69gZ6UCP7ePJp2iDzqAXTVSg+Zu4gS7A1seHrQHdwiq+oNCh72AdDSh2oZ+GDj7eRPWvTvRwmKzFNXxC8YAN+NDVx7dF7fZI28jGCkzqQCf06RrvwgsOor7N/fN/9MFBNuLDDAufNILRsPvTfgP0LsP1SIPKHwAAAABJRU5ErkJggg==" },
		};
		private Dictionary<QTexture, string> resourcesDark = new Dictionary<QTexture, string>()
		{
			{ QTexture.QCheckBoxChecked, "iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAABV0lEQVQoFWO0sLD4zwAETEyMDECEF/wDqvwHIoCABUS42wkwlKRKM3CwM4G4OMGPn/8YemY/Zdh56APQIqA1JSnSDDxczAwszIx4MUgNSC3YdSDncXDgtundhz8Mdx7+hLsCpBakB7cOqNJpS18zpFY9ZFi26R1cM4iBofHM5W/AAICouXzzO8PeY5/BHE1lDogglETROHf1G4bSjicMC9e/Zfjz5z9D9+yXYGXOVrwMhtpcKBrBoQoT0dfgYljG9I5h0bq3DM9f/WZ4/PwXAyfQTxlRojAlcBrFRhNdLoaMSIiiPUc/gRUlhwoziAiimA8WR9EIEgn1EmTQVOFg+A+MZxV5doZAN0GwQnQC0yigimmNcgy7jnxikBZnA8YZuhYInwWUgn78+MfAw82MosLNhg+FD+OA1IL0MIHSXs+cpwxfvv1l+PP3P14MUgNSC9LDSG4iBwCgfYRJ3KYMwgAAAABJRU5ErkJggg==" },
			{ QTexture.QCheckBoxUnchecked, "iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAAAgElEQVQoFe2SywkAIQxE46cHO/Bu/53YgdaguPsCgpd1hb1uIBjxDSNhTEppyF3GGI7XGkNx8ZAhBIkxinNuK+y9S85ZSilicULkvVdX7k8NA8u7xeLNaf3GZFW4PpzOv3CzqW/LIRGnNVlL9ohRa02Ydw0DC6NZJXu11iNTRNQFcsdGKGm8LNQAAAAASUVORK5CYII=" },
			{ QTexture.QDragButton, "iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAAfElEQVQ4EWM0NjZmoCZgoqZhILNGoIEsaGFoDuRroIkR4t4AKjgJU4RuoMb///8XwCSJoRkZGROA6nAaeMPBwYEYc+BqDh48CHIhHGC48MCBA3BJYhhAF4KCaNSFiMDSoHoYQpMBwgrCLLyxDIoteIwRNgtTxQgsHAa/lwH5tiOYn8m38AAAAABJRU5ErkJggg==" },
		};
		private Dictionary<QTexture, string> resourcesLight = new Dictionary<QTexture, string>()
		{
			{ QTexture.QCheckBoxChecked, "iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAABOUlEQVQoFY2Sva6CQBCFz4XVQGFjgiZQGKiJnYmFBQVPAo9l7RvQUZAgvAChpqAiGkNDpQLX2bjkkhuMW+zM7pwv85P58X2/x+swxiBJErmTp+s6PJ9PHmd0Hw4HrFYrzGazSYgCj8cDl8sFSZJQIgZN06AoykeIgrIsc+27Ognz+XwSapoGVVUNcdJSS5+besnDMMTxeESapgNMzj+wKArQEOiUZYk8z7mv6zq34hqBURThdDrhfD6jbVsEQcB1tm3DNE3BcMunKn42mw0vKY5j1HWN2+3G+3ddV0gGO8poWRaESJToOA4Wi8UACGcE0ud+v4dhGOj7Huv1GrvdTmhHdlSqiHiehyzLsFwuJ7eJ0QTv9ztUVRUct9vtdvQWD9ISw2j3rtfr1ytHWmJ4qe/dmyxLZPu75L/vGnGpeAWI1gAAAABJRU5ErkJggg==" },
			{ QTexture.QCheckBoxUnchecked, "iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAAAjklEQVQoFe2STRJDERCEOwzlApzGbVzVabwboCRp9ezeT5J1ZmGU+Roz1Y+U0hPvEBEopbg9jTEGeu+zLlxjjAghwBhzKmKhtYZSCnLOfEjgvYdz7lLEotZ6svvvFKy1t6IFkGVL100t+iD/hQdDWUe/D4c2qrWui24zWWqE3tu27WPLkaVmenX33lcmfwFMazT7V5IT7wAAAABJRU5ErkJggg==" },
			{ QTexture.QDragButton, "iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAATklEQVQ4EWM8ceIEAzUBEzUNA5k1aiDlIcqCxYj/WMTwCTEiS7Js27oVmU8xmxFLOhx1IWnBSv0wxGI/SjLAIo9XaDQv4w0eoiQHfxgCAGQPE/BDNfMZAAAAAElFTkSuQmCC" },
		};

		private Dictionary<QColor, Color> colors;
		private Dictionary<QColor, Color> colorsDark = new Dictionary<QColor, Color>()
		{
			{ QColor.BackgroundDark, new Color(0.15f, 0.15f, 0.15f) },
			{ QColor.Background , new Color(0.22f, 0.22f, 0.22f) },
			{ QColor.Gray       , new Color(0.6f, 0.6f, 0.6f) },
			{ QColor.GrayLight  , new Color(0.8f, 0.8f, 0.8f) },
			{ QColor.GrayDark  , new Color(0.4f, 0.4f, 0.4f) },
		};
		private Dictionary<QColor, Color> colorsLight = new Dictionary<QColor, Color>()
		{
			{ QColor.BackgroundDark, new Color(0.88f, 0.88f, 0.88f) },
			{ QColor.Background , new Color(0.761f, 0.761f, 0.761f) },
			{ QColor.Gray       , new Color(0.3f, 0.3f, 0.3f) },
			{ QColor.GrayLight  , new Color(0.1f, 0.1f, 0.1f) },
			{ QColor.GrayDark  , new Color(0.55f, 0.55f, 0.55f) },
		};

		// CONSTRUCTOR
		private QResources()
		{
			textures = new Dictionary<QTexture, Texture2D>();
			foreach (KeyValuePair<QTexture, string> resourcePair in resourcesCommon)
			{
				Texture2D texture = new Texture2D(0,0, TextureFormat.ARGB32, false, true);
				texture.hideFlags = HideFlags.HideAndDontSave;
				texture.LoadImage(Convert.FromBase64String(resourcePair.Value));
				textures.Add(resourcePair.Key, texture);
			}
			Dictionary<QTexture, string> resources = EditorGUIUtility.isProSkin ? resourcesDark : resourcesLight;
			foreach (KeyValuePair<QTexture, string> resourcePair in resources)
			{
				Texture2D texture = new Texture2D(0,0, TextureFormat.ARGB32, false, true);
				texture.hideFlags = HideFlags.HideAndDontSave;
				texture.LoadImage(Convert.FromBase64String(resourcePair.Value));
				textures.Add(resourcePair.Key, texture);
			}
			colors = EditorGUIUtility.isProSkin ? colorsDark : colorsLight;
		} 

		// PUBLIC
		public Texture2D getTexture(QTexture textureName) 
		{
			return textures[textureName];
		}

		public Color getColor(QColor color)
		{
			return colors[color];
		}
	}
}
