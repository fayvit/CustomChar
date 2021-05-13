using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FayvitMessageAgregator;
using Criatures2021;
using System;

namespace Criatures2021Hud
{
    public class HudChangebleGameElements : MonoBehaviour
    {
        [SerializeField] private Image criatureImg;
        [SerializeField] private Image itemImg;
        [SerializeField] private Image attackImg;
        [SerializeField] private Image containerDeInfoText;
        [SerializeField] private Text infoText;

        private float contTime = 0;
        private float timeTohide = 2;


        // Use this for initialization
        void Start()
        {
            MessageAgregator<MsgChangeSelectedAttack>.AddListener(OnChangeAttack);
            MessageAgregator<MsgChangeSelectedPet>.AddListener(OnChangeSelectedPet);
            MessageAgregator<MsgChangeSelectedItem>.AddListener(OnChangeSelectedItem);
            MessageAgregator<MsgChangeToHero>.AddListener(OnChangeToHero);
            MessageAgregator<MsgChangeToPet>.AddListener(OnChangeToPet);
            MessageAgregator<MsgStartGameElementsHud>.AddListener(OnStartHud);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgChangeSelectedAttack>.RemoveListener(OnChangeAttack);
            MessageAgregator<MsgChangeSelectedPet>.RemoveListener(OnChangeSelectedPet);
            MessageAgregator<MsgChangeSelectedItem>.RemoveListener(OnChangeSelectedItem);
            MessageAgregator<MsgChangeToHero>.RemoveListener(OnChangeToHero);
            MessageAgregator<MsgChangeToPet>.RemoveListener(OnChangeToPet);
            MessageAgregator<MsgStartGameElementsHud>.RemoveListener(OnStartHud);
        }

        private void OnStartHud(MsgStartGameElementsHud obj)
        {
            criatureImg.transform.parent.parent.gameObject.SetActive(true);

            if (obj.petname!=PetName.nulo)
                OnChangeSelectedPet(new MsgChangeSelectedPet() { petname = obj.petname });
            else
                criatureImg.transform.parent.gameObject.SetActive(false);

            if (obj.nameItem != NameIdItem.generico)
                OnChangeSelectedItem(new MsgChangeSelectedItem() { nameItem = obj.nameItem });
            else
                itemImg.transform.parent.gameObject.SetActive(false);

            OnChangeToHero(new MsgChangeToHero());
        }

        private void OnChangeToHero(MsgChangeToHero obj)
        {
            attackImg.transform.parent.gameObject.SetActive(false);
            //criatureImg.transform.parent.parent.gameObject.SetActive(false);
        }

        private void OnChangeToPet(MsgChangeToPet obj)
        {
            //criatureImg.transform.parent.parent.gameObject.SetActive(true);

            if (obj.numCriatures <= 1)
            {
                criatureImg.gameObject.SetActive(false);
            }
            else
            {
                if (obj.petToGoOut != PetName.nulo)
                {
                    criatureImg.gameObject.SetActive(true);
                    criatureImg.sprite = Resources.Load<Sprite>("miniCriatures/" + obj.petToGoOut.ToString());
                }
            }

            attackImg.transform.parent.gameObject.SetActive(true);
            attackImg.sprite = Resources.Load<Sprite>("miniGolpes/" + obj.atkSelected.ToString());
        }



        private void OnChangeSelectedItem(MsgChangeSelectedItem obj)
        {
            itemImg.transform.parent.gameObject.SetActive(true);
            itemImg.sprite = Resources.Load<Sprite>("miniItens/" + obj.nameItem.ToString());
            ActiveInfoText(ItemBase.NomeEmLinguas(obj.nameItem));
        }

        private void OnChangeSelectedPet(MsgChangeSelectedPet obj)
        {
            criatureImg.transform.parent.gameObject.SetActive(true);
            criatureImg.sprite = Resources.Load<Sprite>("miniCriatures/" + obj.petname.ToString());
            ActiveInfoText(PetBase.NomeEmLinguas(obj.petname));
        }

        private void OnChangeAttack(MsgChangeSelectedAttack obj)
        {
            attackImg.sprite = Resources.Load<Sprite>("miniGolpes/" + obj.attackName.ToString());
            ActiveInfoText(PetAttackBase.NomeEmLinguas(obj.attackName));
        }

        private void ActiveInfoText(string s)
        {
            infoText.text = s;
            containerDeInfoText.gameObject.SetActive(true);
            contTime = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (containerDeInfoText.gameObject.activeSelf)
            {
                contTime += Time.deltaTime;
                if (contTime > timeTohide)
                {
                    containerDeInfoText.gameObject.SetActive(false);
                }
            }
        }
    }
}

public struct MsgChangeSelectedPet : IMessageBase {
    public PetName petname;
}
public struct MsgChangeSelectedItem : IMessageBase {
    public NameIdItem nameItem;
}
public struct MsgChangeSelectedAttack : IMessageBase {
    public AttackNameId attackName;
}
public struct MsgStartGameElementsHud : IMessageBase {
    public PetName petname;
    public NameIdItem nameItem;
}