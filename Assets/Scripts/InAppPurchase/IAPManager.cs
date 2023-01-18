using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Purchasing;


public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager instance;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    //Step 1 create your products
    private static string removeAds = "removeads";
    private static string Gems100 = "100_gems";
    private static  string Gems500 = "500_gems";
    private static string Gems1000 = "1000_gems";
    private static string Gems2000 = "2000_gems";

    private GameObject VerifyRemove;
    private GameObject RemoveAds;

    private bool RemovingOfAdsIsChecked = false;

    //************************** Adjust these methods **************************************
    public void InitializePurchasing()
    {
        if (IsInitialized()) { return; }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Step 2 choose if your product is a consumable or non consumable
        builder.AddProduct(removeAds, ProductType.NonConsumable);
        builder.AddProduct(Gems100, ProductType.Consumable);
        builder.AddProduct(Gems500, ProductType.Consumable);
        builder.AddProduct(Gems1000, ProductType.Consumable);
        builder.AddProduct(Gems2000, ProductType.Consumable);



        UnityPurchasing.Initialize(this, builder);
    }


    public bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    //Step 3 Create methods
    public void BuyRemoveAds()
    {
        BuyProductID(removeAds);
    }
    public void BuyGems100()
    {
        BuyProductID(Gems100);
    }
    public void BuyGems500()
    {
        BuyProductID(Gems500);
    }
    public void BuyGems1000()
    {
        BuyProductID(Gems1000);
    }
    public void BuyGems2000()
    {
        BuyProductID(Gems2000);
    }


    //Step 4 modify purchasing
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, removeAds, StringComparison.Ordinal))
        {
            Debug.Log("Remove Succesful");
            //ManagerVars.GetManagerVars().AdsEnabled = false;
            //GameManager.Instance.DisableAds();
            checkProduct(removeAds);
            
        } else if (String.Equals(args.purchasedProduct.definition.id, Gems100, StringComparison.Ordinal))
        {
            Debug.Log("100 gems added");
            GameManager.Instance.AddDiamonForBuy(100);

        }
        else if (String.Equals(args.purchasedProduct.definition.id, Gems500, StringComparison.Ordinal))
        {
            Debug.Log("500 gems added");
            GameManager.Instance.AddDiamonForBuy(500);


        }
        else if (String.Equals(args.purchasedProduct.definition.id, Gems1000, StringComparison.Ordinal))
        {
            Debug.Log("1000 gems added");
            GameManager.Instance.AddDiamonForBuy(1000);


        }
        else if (String.Equals(args.purchasedProduct.definition.id, Gems2000, StringComparison.Ordinal))
        {
            Debug.Log("2000 gems added");
            GameManager.Instance.AddDiamonForBuy(2000);

        }
        else
        {
            Debug.Log("Purchase Failed");
        }
        //GameManager.Instance
        return PurchaseProcessingResult.Complete;
    }

    //
    public void CheckRemoveAdsExternal()
    {
        checkProduct(removeAds);
    }

    public bool GetRemovingOfAdsIsChecked()
    {
        return RemovingOfAdsIsChecked;
    }
    public void SetRemovingOfAdsIsChecked(bool CheckerTrue)
    {
        RemovingOfAdsIsChecked = CheckerTrue;
    }
    //Check NonConsumable Purchases
    public void checkProduct(string product)
    {
        StartCoroutine("CheckProduct", product);
    }

    IEnumerator CheckProduct(string product)
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log(product);
        if (IsInitialized())
        {
            Product cproduct = m_StoreController.products.WithID(product);
            
            if (cproduct != null && cproduct.hasReceipt)
            {

                //GameManager.Instance.RemoveAds = true;
                GameManager.Instance.vars.RemoveAds = true;

                //UnityAdManager.hideBanner();
                AdManager.instance.DestroyBannert();
                //RemoveAds.SetActive(false);
                //VerifyRemove.SetActive(true);
                //GameObject.FindGameObjectWithTag("StorePanel").GetComponent<StorePanel>().DisableBuyRemoveAdsButton();

                StorePanel.instance.DisableBuyRemoveAdsButton();

                //Hide for Admob too

                Debug.Log("Removing of ads is checked: Ads removed");
                RemovingOfAdsIsChecked = true;

            }
            else
            {

                //GameManager.Instance.RemoveAds = false;
                GameManager.Instance.vars.RemoveAds = false;

                Debug.Log("Removing of ads is checked: Ads are showing");
                RemovingOfAdsIsChecked = true;
                //RemoveAds.SetActive(true);
                //VerifyRemove.SetActive(false);
                //GameObject.FindGameObjectWithTag("StorePanel").GetComponent<StorePanel>().DisableBuyRemoveAdsButton();


            }
        }
        else
        {
            //GameManager.Instance.RemoveAds = false;
            GameManager.Instance.vars.RemoveAds = false;
            Debug.Log("Removing of ads wasn't able to be initialized: Ads are showing");
            RemovingOfAdsIsChecked = true;
        }


    }



    


    //**************************** Dont worry about these methods ***********************************
    private void Awake()
    {
        TestSingleton();
        
    }

    void Start()
    {
        if (m_StoreController == null) { InitializePurchasing(); }

        
        //checkProduct(removeAds);
    }

    private void TestSingleton()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                //
                if(result == true)
                {
                    CheckRemoveAdsExternal();
                }
                //
            });
        }else if (Application.platform == RuntimePlatform.Android){
            Debug.Log("RestorePurchases started ...");

            var android = m_StoreExtensionProvider.GetExtension<IGooglePlayStoreExtensions>();
            android.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }



    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        
        RemovingOfAdsIsChecked = true;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}