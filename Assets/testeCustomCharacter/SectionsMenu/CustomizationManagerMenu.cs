using UnityEngine;
using System.Collections;
using FayvitUI;
using System.Collections.Generic;
using FayvitSupportSingleton;
using FayvitMessageAgregator;
using FayvitCommandReader;
using FayvitEventAgregator;
using System;

public class CustomizationManagerMenu : MonoBehaviour
{
    [SerializeField] private CustomizationMenu cMenu;
    [SerializeField] private SectionCustomizationManager secManager;
    [SerializeField] private SectionDataBaseContainer sdbc;
    [SerializeField] private FlagSectionDataBase menusAtivos = FlagSectionDataBase.@base;
    [SerializeField] private TesteMeshCombiner testMeshCombiner;
    [SerializeField] private GetColorHudManager myGetColor;
    [SerializeField] private ColorGridMenu mySuggestionColors;
    [SerializeField] private GlobalColorMenu globalCM;
    [SerializeField] private BasicMenu globalMenu;
    [SerializeField] private float velValForGreyScale = 3;
    [SerializeField] private float velValForColor = 3;

    private ColorDbManager cdbManager;
    private EstadoDoMenu estado = EstadoDoMenu.main;
    private RegistroDeCores transitoryReg;

    public enum EstadoDoMenu
    {
        main,
        colorGrid,
        colorCircle,
        globalizationColors,
        globalizationMenu
    }
    

    private EditableElements[] activeEditables;

    [System.Flags]
    private enum FlagSectionDataBase
    {
        @base = SectionDataBase.@base,
        cabelo = SectionDataBase.cabelo,
        queixo = SectionDataBase.queixo,
        globoOcular = 4,
        pupila = 8,
        iris = 16,
        umidade = 32,
        sobrancelha = 64,
        barba = 128,
        torso = 256,
        mao = 512,
        cintura = 1024,
        pernas = 2048,
        botas = 4096,
        particular = 8192,
        nariz = 16384,
        empty = 32768
    }

    public enum EditableType
    {
        control,
        color,
        mesh,
        texture
    }

    public struct EditableElements
    {
        /// <summary>
        /// DataBase ao qual o objeto é mebro
        /// </summary>
        public SectionDataBase member;
        /// <summary>
        /// Tipo de editavel. Para ser usado na inserção dos itens do container
        /// </summary>
        public EditableType type;
        public string displayName;
        /// <summary>
        /// Indice da coleção(dataBase) ao qual o elemento pertence
        /// </summary>
        public int outIndex;
        /// <summary>
        /// Indice do elemento dentro do seu objeto. utilizavel no caso de várias cores editaveis, varias subseções e etc..
        /// </summary>
        public int inIndex;
        public SectionDataBase mySDB;

    }

    EditableElements[] ActiveEditables
    {
        get
        {
            ChangebleElement changeE = sdbc.GetChangebleElementWithId(SectionDataBase.@base)[0];
            SectionDataBase[] s = changeE.subsection;

            List<EditableElements> retorno = new List<EditableElements>();

            for (int i = 0; i < changeE.coresEditaveis.Length; i++)
            {
                EditableElements thisElement = new EditableElements()
                {
                    mySDB = SectionDataBase.@base,
                    displayName = "Cor " + (i + 1),
                    inIndex = i,
                    outIndex = 0,
                    member = SectionDataBase.@base,
                    type = EditableType.color
                };

                retorno.Add(thisElement);
            }

            for (int i = 0; i < s.Length; i++)
            {
                EditableElements thisElement = new EditableElements()
                {
                    mySDB = s[i],
                    displayName = s[i].ToString(),
                    inIndex = i,
                    outIndex = 0,
                    member = SectionDataBase.@base,
                    type = EditableType.control
                };
                retorno.Add(thisElement);
            }

            for (int i = 0; i < s.Length; i++)
            {
                VerifySubSections(retorno, s[i], 1);
            }

            return retorno.ToArray();
        }
    }

    private void OnUiChange(UiDeOpcoesChangeMessage obj)
    {

        if (cMenu.GetTransformContainer.gameObject == obj.parentOfScrollRect)
        {
            MessageAgregator<MsgChangeMenuDb>.Publish(new MsgChangeMenuDb() { 
                sdb = activeEditables[obj.selectedOption].mySDB
            });
        }
        else
        {
            if (cdbManager != null)
            {
                if (obj.parentOfScrollRect == cdbManager.PaiDoGrid)
                {
                    secManager.ApplyColor(cdbManager.CurrentGridColor);
                }
            }
            else if (obj.parentOfScrollRect == mySuggestionColors.GetTransformContainer.gameObject)
            {
                secManager.ApplyColor(mySuggestionColors.GetSelectedColor);
            }
        }

        
    }

    private void OnChangeInGlobalMessage(ChangeInGlobalColorMessage obj)
    {
        if (obj.optionType == 1)
        {
            secManager.ApplyColor(globalCM.GetSloteColor(obj.registro));
        }
        else
        {
            if (obj.optionType == 0 && obj.check)
            {
                secManager.ApplyColor(globalCM.ChangedColor);
            }else
                secManager.ApplyColor(globalCM.RememberedColor);
        }
    }

    private void OnToggleGlobalColor(ToggleGlobalColorMessage obj)
    {
        secManager.ChangeColorRegIfEqual(obj.indexOfGlobal,globalCM.RememberedColor, globalCM.GetSloteColor(obj.indexOfGlobal));
    }

    private void OnSelectGlobalColor(SelectGlobalColorMessage obj)
    {
        transitoryReg = obj.indexOfGlobal;
        estado = EstadoDoMenu.globalizationMenu;

        EndGlobalColorMenu();
        globalMenu.StartHud(OpcoesDoGlobalizationMenu,
            new string[3] { "Selecionar a cor e não usar registro", "Selecionar a cor e usar o registro", "Voltar para o menu anterior" });

        EditableElements target = activeEditables[cMenu.SelectedOption];
        ColorContainerStruct ccs = secManager.GetColorAssignById(target.member).coresEditaveis[target.inIndex];
        if (ccs.coresEditaveis.registro == obj.indexOfGlobal)
        {
            globalMenu.ChangeSelectionTo(1);
        }
            
        
    }

    private void OpcoesDoGlobalizationMenu(int x)
    {
        switch (x)
        {
            case 0:
                estado = EstadoDoMenu.main;
                secManager.ChangeColorReg(RegistroDeCores.registravel);
            break;
            case 1:
                estado = EstadoDoMenu.main;
                secManager.ChangeColorReg(transitoryReg);
            break;
            case 2:
                globalCM.StartHud(globalCM.RememberedColor,secManager.GuardOriginalColor.cor, secManager.VerifyColorReg(), secManager.GetTargetColorReg);
                StartGlobalColorMenu();
            break;
        }

        globalMenu.FinishHud();
    }

    private void OnButtonMakeGlobal(ButtonMakeGlobalMessage obj)
    {
        secManager.ChangeRegColor(globalCM.RememberedColor, obj.indexOfGlobal);
        globalCM.SetGlobalViewInTheIndex(obj.indexOfGlobal);
    }

    private void OnChangeColorPicker(IGameEvent e)
    {
        Color C = (Color)e.MySendObjects[0];
        secManager.ApplyColor(C);
    }

    int GetInListDbIndex(List<EditableElements> L, SectionDataBase sdb)
    {
        int retorno = -1;
        for (int i = 0; i < L.Count; i++)
        {
            ChangebleElement changeE = sdbc.GetChangebleElementWithId(L[i].member)[L[i].outIndex];

            if (changeE.subsection.Length > 0)
            {
                if (changeE.subsection[L[i].inIndex] == sdb)
                {
                    retorno = i;
                }
            }
        }

        return retorno;
    }

    string DeepView(int deep)
    {
        string s = string.Empty;
        for (int i = 0; i < deep; i++)
        {
            s += " - ";
        }

        return s;
    }

    bool VerifyActiveDbView(SectionDataBase sdb)
    {
        return menusAtivos.HasFlag(StringParaEnum.ObterEnum<FlagSectionDataBase>(sdb.ToString()));
    }

    void VerifySubSections(List<EditableElements> L, SectionDataBase target, int deep)
    {
        if (!VerifyActiveDbView(target))
            return;
        

        int dbIndex = secManager.GetActiveIndexOf(target);

        if (dbIndex == -1)
        {
            Debug.Log("Não encontrada referencia no sectionManager para : " + target);
            return;
        }

        int inListIndex = GetInListDbIndex(L, target);

        ChangebleElement changeE = sdbc.GetChangebleElementWithId(target)[dbIndex];


        List<EditableElements> forInsert = new List<EditableElements>();
        if (changeE is SimpleChangebleMesh || changeE is CombinedChangebleMesh)
        {
            forInsert.Add(new EditableElements()
            {
                mySDB = target,
                displayName = DeepView(deep) + "Type",
                inIndex = 0,
                outIndex = dbIndex,
                member = target,
                type = EditableType.mesh
            });
        }
        else if (changeE is MaskedTexture)
        {
            forInsert.Add(new EditableElements()
            {
                mySDB = target,
                displayName = DeepView(deep) + "Detail",
                inIndex = 0,
                outIndex = dbIndex,
                member = target,
                type = EditableType.texture
            });
        }

        for (int i = 0; i < changeE.coresEditaveis.Length; i++)
        {
            forInsert.Add(new EditableElements()
            {
                mySDB = target,
                displayName = DeepView(deep) + "Cor " + (i + 1),
                inIndex = i,
                outIndex = dbIndex,
                member = target,
                type = EditableType.color
            });
        }

        SectionDataBase[] s = changeE.subsection;

        for (int i = 0; i < s.Length; i++)
        {
            forInsert.Add(new EditableElements()
            {
                mySDB = s[i],
                displayName = DeepView(deep)+" "+s[i].ToString(),
                inIndex = i,
                outIndex = 0,
                member = target,
                type = EditableType.control
            });
        }

        L.InsertRange(inListIndex + 1, forInsert);

        


        if (s.Length > 0)
        {
            for (int i = 0; i < s.Length; i++)
            {
                VerifySubSections(L, s[i], deep + 1);
            }
        }
    }

    string[] GetActiveOption(EditableElements[] elements)
    {
        string[] retorno = new string[elements.Length];
        for (int i = 0; i < elements.Length; i++)
        {
            retorno[i] = elements[i].displayName;
        }

        return retorno;
    }

    int GetElementIndexOf(EditableElements e)
    {
        int retorno = -1;
        for (int i = 0; i < activeEditables.Length; i++)
        {
            EditableElements elegible = activeEditables[i];

            if (elegible.type == e.type
                && elegible.member == e.member
                && elegible.inIndex == e.inIndex)
            {
                retorno = i;
            }

        }

        return retorno;
    }

    private void ChangeAction(int change, int index)
    {
        EditableElements target = activeEditables[index];

        if (target.type == EditableType.control)
        {
            if (change != 0)
            {
                SectionDataBase sdb = sdbc.GetChangebleElementWithId(target.member)[target.outIndex].subsection[target.inIndex];
                FlagSectionDataBase flag = StringParaEnum.ObterEnum<FlagSectionDataBase>(sdb.ToString());
                
                if (change > 0 && !menusAtivos.HasFlag(flag))
                {
                    menusAtivos |= flag;

                }
                else if (change < 0 && menusAtivos.HasFlag(flag))
                {
                    menusAtivos &= ~flag;
                }

                RestartMenu(target);

            }
        } else if (target.type==EditableType.mesh||target.type==EditableType.texture)
        {
            if(change!=0)
                ChangeElementMainAction(target, change);
        }
    }

    EditableElements GetEditableElementBySdb(SectionDataBase sdb)
    {
        EditableElements retorno=new EditableElements();
        for (int i = 0; i < activeEditables.Length; i++)
        {
            if (activeEditables[i].mySDB == sdb && activeEditables[i].type == EditableType.control)
            {
                retorno = activeEditables[i];
            }
        }

        return retorno;
    }

    void EscapeAction(int index)
    {
        EditableElements target = activeEditables[index];

        if (target.type != EditableType.control && target.member != SectionDataBase.@base)
        {
            FlagSectionDataBase flag = StringParaEnum.ObterEnum<FlagSectionDataBase>(target.member.ToString());
            if (menusAtivos.HasFlag(flag))
            {
                menusAtivos &= ~flag;

                RestartMenu(GetEditableElementBySdb(target.member));
            }
        }
        else if (target.type == EditableType.control)
        {
            SectionDataBase sdb = sdbc.GetChangebleElementWithId(target.member)[target.outIndex].subsection[target.inIndex];
            FlagSectionDataBase flag = StringParaEnum.ObterEnum<FlagSectionDataBase>(sdb.ToString());
            if (menusAtivos.HasFlag(flag))
            {
                menusAtivos &= ~flag;

                RestartMenu(target);
            }
            else if (target.member != SectionDataBase.@base)
                EscapeAction(index-1);
        }
    }

    private void MainAction(int index)
    {

        EditableElements target = activeEditables[index];

        if (target.type == EditableType.control)
        {
            ControlMainAction(target);

        }
        else if (target.type == EditableType.mesh || target.type == EditableType.texture)
        {
            ChangeElementMainAction(target);
        }
        else if (target.type == EditableType.color)
        {
            ColorContainerStruct ccs = secManager.GetColorAssignById(target.member).coresEditaveis[target.inIndex];

            secManager.StartChangeColor(target.member, target.inIndex, ccs);
            ChangeColorMainAction(ccs);
            estado = EstadoDoMenu.colorGrid;
        }
    }

    private void ChangeColorMainAction(ColorContainerStruct ccs)
    {
               
        SupportSingleton.Instance.InvokeOnEndFrame(() =>
        {
            Color[] C = DbColors.ColorsByDb(ccs.coresEditaveis.registro);
            int selectIndex = DbColors.GetApproximateColorIndex(C, ccs.cor);
            mySuggestionColors.StartHud((int x) => { }, C,selectIndex);
            secManager.ApplyColor(C[selectIndex]);

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<MsgOpenColorMenu>.Publish(new MsgOpenColorMenu() { reg = ccs.coresEditaveis.registro});
            });        

        });
    }

    void ChangeElementMainAction(EditableElements target,int changeVal = 1)
    {
        ChangebleElement[] ce = sdbc.GetChangebleElementWithId(target.member);

        if (ce[0] is SimpleChangebleMesh)
        {
            secManager.TrocaMesh(target.member, ce as SimpleChangebleMesh[], changeVal);
        }
        else if (ce[0] is CombinedChangebleMesh)
        {
            CombinedChangebleMesh[] ccm = ce as CombinedChangebleMesh[];
            CombinedChangebleMesh[] doComb = sdbc.GetCombinedMeshDbWithID(ccm[0].combinedWithDb);
            secManager.ChangeCombinedMesh(target.member, ccm, doComb, changeVal);
        }
        else if (ce[0] is MaskedTexture)
        {
            secManager.ChangeTextureElement(target.member, ce as MaskedTexture[], changeVal);
        }

        RestartMenu(target);
    }

    void ControlMainAction(EditableElements target)
    {
        SectionDataBase sdb = sdbc.GetChangebleElementWithId(target.member)[target.outIndex].subsection[target.inIndex];
        FlagSectionDataBase flag = StringParaEnum.ObterEnum<FlagSectionDataBase>(sdb.ToString());

        if (menusAtivos.HasFlag(flag))
        {
            menusAtivos &= ~flag;
        }
        else
        {
            menusAtivos |= flag;
        }

        RestartMenu(target);
    }

    void RestartMenu(EditableElements target)
    {
        activeEditables = ActiveEditables;

        cMenu.FinishHud();
        int indexOfElement = GetElementIndexOf(target);
        cMenu.StartHud(secManager,MainAction, ChangeAction, (int x) => { }, activeEditables, selectIndex: indexOfElement);
    }

    // Use this for initialization
    void Start()
    {
        //menusAtivos &= ~FlagSectionDataBase.pupila;

        if(secManager is null)
            secManager = FindObjectOfType<SectionCustomizationManager>();
        
        if(sdbc is null)
            sdbc = FindObjectOfType<SectionDataBaseContainer>();

        //string[] StartOptions = GetActiveOption(ActiveEditables);
        activeEditables = ActiveEditables;
        cMenu.StartHud(secManager,MainAction, ChangeAction, (int x) => { },activeEditables);

        MessageAgregator<MsgChangeMenuDb>.Publish(new MsgChangeMenuDb()
        {
            sdb = activeEditables[0].mySDB
        });

        MessageAgregator<UiDeOpcoesChangeMessage>.AddListener(OnUiChange);
    }

    private void OnDestroy()
    {
        MessageAgregator<UiDeOpcoesChangeMessage>.RemoveListener(OnUiChange);
    }

    bool MainState()
    {
        bool up = Input.GetKeyDown(KeyCode.W);
        bool down = Input.GetKeyDown(KeyCode.S);
        bool left = Input.GetKeyDown(KeyCode.A);
        bool right = Input.GetKeyDown(KeyCode.D);
        int change = (up ? -1 : 0) + (down ? 1 : 0);
        int hChange = (left ? -1 : 0) + (right ? 1 : 0);
        cMenu.ChangeOption(change);
        ChangeAction(hChange, cMenu.SelectedOption);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            MainAction(cMenu.SelectedOption);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapeAction(cMenu.SelectedOption);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CustomizationContainerDates ccd = secManager.GetCustomDates();
            ccd.Save();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CustomizationContainerDates ccd = new CustomizationContainerDates();
            ccd.Load();

            secManager.SetCustomDates(ccd);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            testMeshCombiner.StartCombiner(secManager);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ToSaveCustomizationContainer.Instance.Save(secManager.GetCustomDates());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ToSaveCustomizationContainer.Instance.Load();
            List<CustomizationContainerDates> lccd = ToSaveCustomizationContainer.Instance.ccds;
            if (lccd != null && lccd.Count > 0)
            {
                int i = UnityEngine.Random.Range(0, lccd.Count);
                secManager.SetCustomDates(lccd[i]);
            }
            else
            {
                Debug.Log(lccd);
                Debug.Log(lccd.Count);
            }

            secManager.gameObject.SetActive(false);
            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                secManager.gameObject.SetActive(true);
            });
        }

        return false;
    }

    bool ColorState()
    {
        bool foi = false;
        bool effective = false;

        int x = CommandReader.GetIntTriggerDown("horizontal", Controlador.teclado);
        int y = CommandReader.GetIntTriggerDown("vertical", Controlador.teclado);
        
        mySuggestionColors.ChangeOption(y,x);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            effective = false;
            foi = true;
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            effective = true;
            foi = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
            Color C = mySuggestionColors.GetSelectedColor;

            FinishColorGrid();
            
            ChangeToColorCircle(C);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && secManager.GetTargetColorReg != RegistroDeCores.skin)
        {
            globalCM.StartHud(mySuggestionColors.GetSelectedColor, secManager.GuardOriginalColor.cor, secManager.VerifyColorReg(), secManager.GetTargetColorReg);
            StartGlobalColorMenu();
            FinishColorGrid();
        }

        if (foi)
        {
            secManager.EndChangeColor(effective);

            FinishColorGrid();

            estado = EstadoDoMenu.main;
        }
        return true;
    }

    private void FinishColorGrid()
    {
        mySuggestionColors.FinishHud();
    }

    private void ChangeToColorCircle(Color C)
    {
        myGetColor.transform.parent.gameObject.SetActive(true);
        estado = EstadoDoMenu.colorCircle;
        
        EventAgregator.AddListener(EventKey.changeColorPicker, OnChangeColorPicker);

        SupportSingleton.Instance.InvokeOnEndFrame(() =>
        {
            myGetColor.SetColor(C);

            EditableElements target = activeEditables[cMenu.SelectedOption];
            
            MessageAgregator<MsgOpenColorMenu>.Publish(new MsgOpenColorMenu() { reg = secManager.GetTargetColorReg });
            
        });
    }

    void StartGlobalColorMenu()
    {

        estado = EstadoDoMenu.globalizationColors;
        MessageAgregator<ChangeInGlobalColorMessage>.AddListener(OnChangeInGlobalMessage);
        MessageAgregator<ButtonMakeGlobalMessage>.AddListener(OnButtonMakeGlobal);
        MessageAgregator<SelectGlobalColorMessage>.AddListener(OnSelectGlobalColor);
        MessageAgregator<ToggleGlobalColorMessage>.AddListener(OnToggleGlobalColor);
    }

    private bool ColorCircleState()
    {
        bool foi = false;
        bool effective = false;

        Vector2 V = new Vector2(
            CommandReader.GetAxis("horizontal", Controlador.teclado),
            CommandReader.GetAxis("vertical", Controlador.teclado)
            );

        float val = (Input.GetKey(KeyCode.Q) ? 1 : 0) + (Input.GetKey(KeyCode.E) ? -1 : 0);
        val *= Time.deltaTime * velValForGreyScale;

        myGetColor.MoveMark(V*velValForColor,val);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            effective = false;
            foi = true;
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            effective = true;
            foi = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EndColorCircle();
            ChangeToColorGrid();

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && secManager.GetTargetColorReg!=RegistroDeCores.skin)
        {
            EndColorCircle();
            globalCM.StartHud(myGetColor.CurrentColor, secManager.GuardOriginalColor.cor, secManager.VerifyColorReg(), secManager.GetTargetColorReg);
            StartGlobalColorMenu();
        }

        if (foi)
        {
            EndColorCircle();
            secManager.EndChangeColor(effective);
            estado = EstadoDoMenu.main;
        }



        return true;
    }

    void EndColorCircle()
    {
        myGetColor.transform.parent.gameObject.SetActive(false);
        EventAgregator.RemoveListener(EventKey.changeColorPicker, OnChangeColorPicker);
    }

    private void EndGlobalColorMenu()
    {
        globalCM.FinishHud();

        SupportSingleton.Instance.InvokeOnEndFrame(() =>
        {
            MessageAgregator<ChangeInGlobalColorMessage>.RemoveListener(OnChangeInGlobalMessage);
            MessageAgregator<ButtonMakeGlobalMessage>.RemoveListener(OnButtonMakeGlobal);
            MessageAgregator<SelectGlobalColorMessage>.RemoveListener(OnSelectGlobalColor);
            MessageAgregator<ToggleGlobalColorMessage>.RemoveListener(OnToggleGlobalColor);
        });
    }

    private bool GlobalizationColorsState()
    {
        globalCM.ChangeOption(
            CommandReader.GetIntTriggerDown("horizontal", Controlador.teclado),
            -CommandReader.GetIntTriggerDown("vertical", Controlador.teclado)
            );

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            secManager.EndChangeColor(false);
            estado = EstadoDoMenu.main;
            EndGlobalColorMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            globalCM.InvokeSelectedAction();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EndGlobalColorMenu();
            ChangeToColorGrid();

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            
            ChangeToColorCircle(globalCM.RememberedColor);
            EndGlobalColorMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            EndGlobalColorMenu();
            estado = EstadoDoMenu.main;
        }


        return true;
    }

    void ChangeToColorGrid()
    {
        EditableElements target = activeEditables[cMenu.SelectedOption];
        ColorContainerStruct ccs = secManager.GetColorAssignById(target.member).coresEditaveis[target.inIndex];
        ChangeColorMainAction(ccs);
        estado = EstadoDoMenu.colorGrid;
    }

    private bool GlobalizationMenuState()
    {
        int val = -CommandReader.GetIntTriggerDown("vertical", Controlador.teclado);
        globalMenu.ChangeOption(val);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OpcoesDoGlobalizationMenu(globalMenu.SelectedOption);
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        bool conciso = estado switch
        {
            EstadoDoMenu.main => MainState(),
            EstadoDoMenu.colorGrid => ColorState(),
            EstadoDoMenu.globalizationColors => GlobalizationColorsState(),
            EstadoDoMenu.colorCircle => ColorCircleState(),
            EstadoDoMenu.globalizationMenu=>GlobalizationMenuState(),
            _ => false
        };
        
    }
}

[System.Serializable]
public class CustomizationMenu : InteractiveUiBase
{
    private SectionCustomizationManager secManager;
    private CustomizationManagerMenu.EditableElements[] opcoes;
    private System.Action<int> mainAction;
    private System.Action<int,int> changeAction;
    private System.Action<int> returnAction;
    private bool estadoDeAcao = false;

    protected System.Action<int> MainAction
    {
        get { return mainAction; }
    }

    public void StartHud(
        SectionCustomizationManager secManager,
        System.Action<int> mainAction,
        System.Action<int,int> changeAction,
        System.Action<int> returnAction,
        CustomizationManagerMenu.EditableElements[] dasOpcoes,
        ResizeUiType tipoDeR = ResizeUiType.vertical,
        int selectIndex =0
        )
    {
        this.secManager = secManager;
        this.opcoes = dasOpcoes;

        this.returnAction += returnAction;
        this.changeAction += changeAction;

        this.mainAction += (int x) =>
        {
            if (!estadoDeAcao)
            {
                estadoDeAcao = true;
                ChangeSelectionTo(x);
                

                SupportSingleton.Instance.InvokeInRealTime(() =>
                {
                    Debug.Log("Função chamada com delay para destaque do botão");
                    mainAction(x);
                    estadoDeAcao = false;
                }, .05f);
            }
        };
        StartHud(opcoes.Length, tipoDeR,selectIndex);
    }

    public override void SetContainerItem(GameObject G, int indice)
    {
        A_CustomizationOption uma = G.GetComponent<A_CustomizationOption>();
        uma.SetOptions(mainAction,returnAction,changeAction, opcoes[indice],secManager);
    }

    protected override void AfterFinisher()
    {
        mainAction = null;
        changeAction = null;
        returnAction = null;
        //Seria preciso uma finalização especifica??
    }
}