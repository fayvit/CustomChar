using FayvitCam;
using FayvitCommandReader;
using FayvitMessageAgregator;
using UnityEngine;

public class CustomizationCamManager : MonoBehaviour
{
    [SerializeField] private Vector3 velMinMaxZ;
    [SerializeField] private Vector3 velMinMaxY;

    private SetOfSectionDB setDb = SetOfSectionDB.tronco;
    private DatesForCam dtForCam;

    private bool mudandoCam;
    private float tempoDecorrido = 0;
    private float startDistance = 0;
    private float startHeight = 0;
    private const float TEMPO_MUDANDO_CAM = .375F;
    

    // Start is called before the first frame update
    void Start()
    {
        MessageAgregator<MsgChangeMenuDb>.AddListener(OnChangeMenuDb);
    }

    private void OnDestroy()
    {
        MessageAgregator<MsgChangeMenuDb>.RemoveListener(OnChangeMenuDb);
    }

    private void OnChangeMenuDb(MsgChangeMenuDb obj)
    {
        SetOfSectionDB setDb = CustomizatioDatesForCam.GetDataBaseCamSet(obj.sdb);
        if (setDb != this.setDb)
        {
            this.setDb = setDb;
            mudandoCam = true;
            tempoDecorrido = 0;
            startDistance = CameraApplicator.cam.Cdir.SphericalDistance;
            startHeight = CameraApplicator.cam.Cdir.VarVerticalHeightPoint;
            dtForCam = CustomizatioDatesForCam.GetDates(obj.sdb);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DirectionalCamera cDir = CameraApplicator.cam.Cdir;
        if (mudandoCam)
        {
            tempoDecorrido += Time.deltaTime;
            cDir.SphericalDistance = Mathf.Lerp(startDistance, dtForCam.distance, tempoDecorrido / TEMPO_MUDANDO_CAM);
            cDir.VarVerticalHeightPoint = Mathf.Lerp(startHeight, dtForCam.height, tempoDecorrido / TEMPO_MUDANDO_CAM);

            if (tempoDecorrido >= TEMPO_MUDANDO_CAM)
                mudandoCam = false;

        }
        else
        {
            Vector3 V = new Vector3(
                -CommandReader.GetAxis("Xcam", Controlador.teclado),
                -CommandReader.GetAxis("Zcam", Controlador.teclado),
                CommandReader.GetAxis("Ycam", Controlador.teclado)
                );

            if (CameraApplicator.cam)
            {
                
                CameraApplicator.cam.ValoresDeCamera(V.x, 0, false, false);
                float f = cDir.SphericalDistance + V.z * velMinMaxZ.x * Time.deltaTime;
                cDir.SphericalDistance = Mathf.Clamp(f, velMinMaxZ.y, velMinMaxZ.z);
                f = cDir.VarVerticalHeightPoint + V.y * velMinMaxY.x * Time.deltaTime;
                cDir.VarVerticalHeightPoint = Mathf.Clamp(f, velMinMaxY.y, velMinMaxY.z);
            }
            else
                Debug.Log("Ué");
        }
    }
}

public struct MsgChangeMenuDb : IMessageBase
{
    public SectionDataBase sdb;
}

public struct DatesForCam
{
    public float distance;
    public float height;
}

public enum SetOfSectionDB
{ 
    cabeca,
    tronco,
    membros,
    allView
}


public class CustomizatioDatesForCam
{
    public static DatesForCam GetDates(SectionDataBase sdb)
    {
        SetOfSectionDB sosDb = GetDataBaseCamSet(sdb);
        return sosDb switch
        {
            SetOfSectionDB.cabeca => new DatesForCam() { distance = .8f, height = .7f },
            SetOfSectionDB.tronco => new DatesForCam() { distance = 1.5f, height = .32f },
            SetOfSectionDB.membros => new DatesForCam() { distance = 1.5f, height = -0.27f },
            SetOfSectionDB.allView => new DatesForCam() { distance = 1.8f, height = 0f },
            _ => new DatesForCam() { distance = .8f, height = .7f }
        };
    }

    public static SetOfSectionDB GetDataBaseCamSet(SectionDataBase sdb)
    {

        return sdb switch
        {
            SectionDataBase.barba       => SetOfSectionDB.cabeca,
            SectionDataBase.@base       => SetOfSectionDB.cabeca,
            SectionDataBase.cabelo      => SetOfSectionDB.cabeca,
            SectionDataBase.globoOcular => SetOfSectionDB.cabeca,
            SectionDataBase.iris        => SetOfSectionDB.cabeca,
            SectionDataBase.nariz       => SetOfSectionDB.cabeca,
            SectionDataBase.pupila      => SetOfSectionDB.cabeca,
            SectionDataBase.queixo      => SetOfSectionDB.cabeca,
            SectionDataBase.umidade     => SetOfSectionDB.cabeca,
            SectionDataBase.sobrancelha => SetOfSectionDB.cabeca,
            SectionDataBase.torso       =>SetOfSectionDB.tronco,
            SectionDataBase.mao         => SetOfSectionDB.tronco,
            SectionDataBase.pernas      => SetOfSectionDB.membros,
            SectionDataBase.botas       => SetOfSectionDB.membros,
            SectionDataBase.cintura     => SetOfSectionDB.membros,
            SectionDataBase.empty       => SetOfSectionDB.allView,
            _                           => SetOfSectionDB.cabeca
        };
    }
}
