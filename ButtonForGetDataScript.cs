using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ButtonForGetDataScript : MonoBehaviour
{
    // webRequest を使って AWS からデータを取得する．
    // 参考資料：https://qiita.com/ponchan/items/65aeb43e8fea8da0bcac
    private const string URL = "https://u9o2yq1n5j.execute-api.ap-northeast-1.amazonaws.com/sample";
    public void OnClick()
    {
        StartCoroutine("OnSend", URL);
    }

    //コルーチン
    IEnumerator OnSend(string url)
    {
        //URLをGETで用意
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        //レスポンスが戻ってくるまで待機
        yield return webRequest.SendWebRequest();

        //エラーが出ていないかチェック
        if (webRequest.isNetworkError)
        {
            //通信失敗
            Debug.Log("Error!: " + webRequest.error);
        }
        else
        {
            //通信成功
            string dataStr = webRequest.downloadHandler.text;
            Debug.Log("Successed!: " + dataStr);

            BodyJson bodyJson = JsonUtility.FromJson<BodyJson>(dataStr);
            List<float> x = bodyJson.pca_data.x;
            List<float> y = bodyJson.pca_data.y;
            List<float> z = bodyJson.pca_data.z;

            string xStr = "";
            string yStr = "";
            string zStr = "";
            for (int i = 0; i < 3; i++)
            {
                xStr += x[i].ToString() + ", ";
                yStr += y[i].ToString() + ", ";
                zStr += z[i].ToString() + ", ";
            }
            Debug.Log("x = " + xStr);
            Debug.Log("y = " + yStr);
            Debug.Log("z = " + zStr);
        }
    }

    // AWS から取得するデータの形式を規定するクラス
    [System.Serializable]
    class BodyJson
    {
        public int statusCode;
        public PosJson pca_data;
    }

    [System.Serializable]
    class PosJson
    {
        public List<float> x;
        public List<float> y;
        public List<float> z;
    }
}
