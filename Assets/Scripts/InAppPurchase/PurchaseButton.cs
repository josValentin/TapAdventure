using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseButton : MonoBehaviour
{

    public enum PurchaseType {removeAds, gems100, gems500, gems1000, gems2000};
    public PurchaseType purchaseType;

    public void ClickPurchaseButton()
    {
        switch (purchaseType)
        {
            case PurchaseType.removeAds:
                IAPManager.instance.BuyRemoveAds();
                break;
            case PurchaseType.gems100:
                IAPManager.instance.BuyGems100();

                break;
            case PurchaseType.gems500:
                IAPManager.instance.BuyGems500();
                break;
            case PurchaseType.gems1000:
                IAPManager.instance.BuyGems1000();
                break;
            case PurchaseType.gems2000:
                IAPManager.instance.BuyGems2000();
                break;
        }
    }
}
